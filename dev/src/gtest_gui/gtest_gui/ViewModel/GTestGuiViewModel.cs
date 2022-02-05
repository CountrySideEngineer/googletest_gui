using gtest_gui.Command;
using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest_gui.MoveWindow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CountrySideEngineer.ViewModel.Base;

namespace gtest_gui.ViewModel
{
	/// <summary>
	/// Main view model class of gtest_gui application.
	/// </summary>
	public class GTestGuiViewModel : ViewModelBase
	{
		/// <summary>
		/// Test file path field.
		/// </summary>
		protected string _testFilePath;

		/// <summary>
		/// Title of application shown in tilte bar.
		/// </summary>
		protected string _applicationTitle;

		/// <summary>
		/// Field of which a test can run or not.
		/// </summary>
		protected bool _canRunTest;

		/// <summary>
		/// Field of which a test data can reload or not.
		/// </summary>
		protected bool _canReloadTest;

		/// <summary>
		/// Current selected test index.
		/// </summary>
		protected int _selectedTestIndex;

		/// <summary>
		/// Field of test information.
		/// </summary>
		protected TestInformation _testInfo;

		/// <summary>
		/// Delegate command to set target test file.
		/// </summary>
		protected DelegateCommand _setTestFileByUserCommand;

		/// <summary>
		/// Delegate command to change the test runnable state.
		/// </summary>
		protected DelegateCommand _changeTestSelectedByUserCommand;

		protected DelegateCommand _runTestCommand;

		protected DelegateCommand _showHistoryCommand;

		/// <summary>
		/// Load test data from test execution file.
		/// </summary>
		protected DelegateCommand _loadTestCommand;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GTestGuiViewModel()
		{
			this.TestInfo = new TestInformation();
			this.CanRunTest = false;
			this.CanReloadTest = false;
		}

		/// <summary>
		/// Application title in application bar.
		/// </summary>
		public string ApplicationTitle
		{
			get
			{
				string applicationTitle = "gtest_gui";
				if ((!string.IsNullOrEmpty(this.TestInfo.TestFile)) &&
					(!string.IsNullOrWhiteSpace(this.TestInfo.TestFile)))
				{
					applicationTitle += $" - {this.TestInfo.TestFile}";
				}
				return applicationTitle;
			}
		}

		/// <summary>
		/// Test information.
		/// </summary>
		public TestInformation TestInfo
		{
			get
			{
				return this._testInfo;
			}
			set
			{
				this._testInfo = value;
				this.RaisePropertyChanged(nameof(TestInfo));
				this.RaisePropertyChanged(nameof(ApplicationTitle));
			}
		}

		/// <summary>
		/// Gets a values indicating whether the tests listed on and selected by user can run or not.
		/// </summary>
		public bool CanRunTest
		{
			get
			{
				return this._canRunTest;
			}
			set
			{
				this._canRunTest = value;
				this.RaisePropertyChanged("CanRunTest");
			}
		}

		/// <summary>
		/// Gets a value indicating whether the test datas can be reload or not.
		/// </summary>
		public bool CanReloadTest
		{
			get
			{
				return this._canReloadTest;
			}
			set
			{
				this._canReloadTest = value;
				this.RaisePropertyChanged("CanReloadTest");
			}
		}

		/// <summary>
		/// Current selected test index.
		/// </summary>
		public int SelectedTestIndex
		{
			get
			{
				return this._selectedTestIndex;
			}
			set
			{
				this._selectedTestIndex = value;
				this.RaisePropertyChanged(nameof(SelectedTestIndex));
			}
		}

		/// <summary>
		/// Command to let user to select target test file.
		/// </summary>
		public DelegateCommand SetTestFileByUserCommand
		{
			get
			{
				if (null == this._setTestFileByUserCommand)
				{
					this._setTestFileByUserCommand = new DelegateCommand(this.SetTestFileByUserCommandExecute);
				}
				return this._setTestFileByUserCommand;
			}
		}

		/// <summary>
		/// Command to let user to run test.
		/// </summary>
		public DelegateCommand ChangeTestSelectedByUserCommand
		{
			get
			{
				if (null == this._changeTestSelectedByUserCommand)
				{
					this._changeTestSelectedByUserCommand = new DelegateCommand(this.ChangeTestSelectedByUserCommandExecute);
				}
				return this._changeTestSelectedByUserCommand;
			}
		}

		/// <summary>
		/// Execute test.
		/// </summary>
		public DelegateCommand RunTestCommand
		{
			get
			{
				if (null == this._runTestCommand)
				{
					this._runTestCommand = new DelegateCommand(this.RunTestCommandExecute);
				}
				return this._runTestCommand;
			}
		}

		/// <summary>
		/// Load test data from file.
		/// </summary>
		public DelegateCommand LoadTestCommand
		{
			get
			{
				if (null == this._loadTestCommand)
				{
					this._loadTestCommand = new DelegateCommand(this.LoadTestCommandExecute);
				}
				return this._loadTestCommand;
			}
		}

		public DelegateCommand ShowHistoryCommand
		{
			get
			{
				if (null == this._showHistoryCommand)
				{
					this._showHistoryCommand = new DelegateCommand(this.ShowHistoryCommandExecute);
				}
				return this._showHistoryCommand;
			}
		}

		/// <summary>
		/// Actual command function to select target test file.
		/// </summary>
		public void SetTestFileByUserCommandExecute()
		{
			var dialog = new OpenFileDialog
			{
				Title = "ファイルを開く",
				Filter = "(*.exe)|*.exe"
			};
			if (true == dialog.ShowDialog())
			{
				this.TestInfo.TestFile = dialog.FileName;

				this.LoadTestCommandExecute();
			}
		}

		/// <summary>
		/// Actual command function to change the 
		/// </summary>
		public void ChangeTestSelectedByUserCommandExecute()
		{
			var selectedTest = this.TestInfo.TestItems.Where(_ => _.IsSelected == true);
			if (0 < selectedTest.Count())
			{
				this.CanRunTest = true;
			}
			else
			{
				this.CanRunTest = false;
			}
		}

		/// <summary>
		/// Update flags meaning the commands this page provides can execute or not.
		/// </summary>
		protected void UpdateCanCommandExecute()
		{
			//Update parameter flags whether commands the view model provides can execute.
			//1. Reload command
			int testInfoItemCount = this.TestInfo.TestItems.Count();
			if (0 < testInfoItemCount)
			{
				this.CanReloadTest = true;
			}

			//2. Test execute command.
			ChangeTestSelectedByUserCommandExecute();
		}

		/// <summary>
		/// Execute test.
		/// </summary>
		public void RunTestCommandExecute()
		{
			//var command = new TestExecuteCommand();
			var command = new TestExecuteAsyncCommand();
			var argument = new TestCommandArgument(this.TestInfo);
			this.ExecuteCommand(command, argument);
			this.LoadTestCommandExecute();
		}

		/// <summary>
		/// Load test from file.
		/// </summary>
		public void LoadTestCommandExecute()
		{
			try
			{
				var command = new LoadTestLogCommand();
				var argument = new TestCommandArgument(this.TestInfo);
				TestInformation testInformation = (TestInformation)ExecuteCommand(command, argument);
				if (null != this.TestInfo)
				{
					foreach (var testItem in this.TestInfo.TestItems)
					{
						var newTestItem = testInformation.TestItems
							.Where(_ => _.Equals(testItem))
							.FirstOrDefault();
						newTestItem.IsSelected = testItem.IsSelected;
					}

				}
				this.TestInfo = testInformation;

				this.UpdateCanCommandExecute();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Execute command.
		/// </summary>
		/// <param name="command">Command object to execute.</param>
		/// <param name="commandArg">Command argument.</param>
		/// <returns>Result of command.</returns>
		protected object ExecuteCommand(ITestCommand command, TestCommandArgument commandArg)
		{
			object cmdResult = command.ExecuteCommand(commandArg);
			return cmdResult;
		}

		public void ShowHistoryCommandExecute()
		{
			var mover = new Move2History();
			mover.Move(this);
		}
	}
}
