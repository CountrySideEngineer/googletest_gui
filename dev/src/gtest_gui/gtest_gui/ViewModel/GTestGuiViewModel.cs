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
			TestInfo = new TestInformation();
			CanRunTest = false;
			CanReloadTest = false;
		}

		/// <summary>
		/// Application title in application bar.
		/// </summary>
		public string ApplicationTitle
		{
			get
			{
				string applicationTitle = "gtest_gui";
				if ((!string.IsNullOrEmpty(TestInfo.TestFile)) &&
					(!string.IsNullOrWhiteSpace(TestInfo.TestFile)))
				{
					applicationTitle += $" - {TestInfo.TestFile}";
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
				return _testInfo;
			}
			set
			{
				_testInfo = value;
				RaisePropertyChanged(nameof(TestInfo));
				RaisePropertyChanged(nameof(ApplicationTitle));
			}
		}

		/// <summary>
		/// Gets a values indicating whether the tests listed on and selected by user can run or not.
		/// </summary>
		public bool CanRunTest
		{
			get
			{
				return _canRunTest;
			}
			set
			{
				_canRunTest = value;
				RaisePropertyChanged("CanRunTest");
			}
		}

		/// <summary>
		/// Gets a value indicating whether the test datas can be reload or not.
		/// </summary>
		public bool CanReloadTest
		{
			get
			{
				return _canReloadTest;
			}
			set
			{
				_canReloadTest = value;
				RaisePropertyChanged("CanReloadTest");
			}
		}

		/// <summary>
		/// Current selected test index.
		/// </summary>
		public int SelectedTestIndex
		{
			get
			{
				return _selectedTestIndex;
			}
			set
			{
				_selectedTestIndex = value;
				RaisePropertyChanged(nameof(SelectedTestIndex));
			}
		}

		/// <summary>
		/// Command to let user to select target test file.
		/// </summary>
		public DelegateCommand SetTestFileByUserCommand
		{
			get
			{
				if (null == _setTestFileByUserCommand)
				{
					_setTestFileByUserCommand = new DelegateCommand(SetTestFileByUserCommandExecute);
				}
				return _setTestFileByUserCommand;
			}
		}

		/// <summary>
		/// Command to let user to run test.
		/// </summary>
		public DelegateCommand ChangeTestSelectedByUserCommand
		{
			get
			{
				if (null == _changeTestSelectedByUserCommand)
				{
					_changeTestSelectedByUserCommand = new DelegateCommand(ChangeTestSelectedByUserCommandExecute);
				}
				return _changeTestSelectedByUserCommand;
			}
		}

		/// <summary>
		/// Execute test.
		/// </summary>
		public DelegateCommand RunTestCommand
		{
			get
			{
				if (null == _runTestCommand)
				{
					_runTestCommand = new DelegateCommand(RunTestCommandExecute);
				}
				return _runTestCommand;
			}
		}

		/// <summary>
		/// Load test data from file.
		/// </summary>
		public DelegateCommand LoadTestCommand
		{
			get
			{
				if (null == _loadTestCommand)
				{
					_loadTestCommand = new DelegateCommand(LoadTestCommandExecute);
				}
				return _loadTestCommand;
			}
		}

		public DelegateCommand ShowHistoryCommand
		{
			get
			{
				if (null == _showHistoryCommand)
				{
					_showHistoryCommand = new DelegateCommand(ShowHistoryCommandExecute);
				}
				return _showHistoryCommand;
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
				TestInfo.TestFile = dialog.FileName;

				LoadTestCommandExecute();
			}
		}

		/// <summary>
		/// Actual command function to change the 
		/// </summary>
		public void ChangeTestSelectedByUserCommandExecute()
		{
			IEnumerable<TestItem> selectedTest = TestInfo.TestItems.Where(_ => _.IsSelected);
			if (0 < selectedTest.Count())
			{
				CanRunTest = true;
			}
			else
			{
				CanRunTest = false;
			}
		}

		/// <summary>
		/// Update flags meaning the commands this page provides can execute or not.
		/// </summary>
		protected void UpdateCanCommandExecute()
		{
			//Update parameter flags whether commands the view model provides can execute.
			//1. Reload command
			int testInfoItemCount = TestInfo.TestItems.Count();
			if (0 < testInfoItemCount)
			{
				CanReloadTest = true;
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
			var argument = new TestCommandArgument(TestInfo);
			_ = ExecuteCommand(command, argument);
			LoadTestCommandExecute();
		}

		/// <summary>
		/// Load test from file.
		/// </summary>
		public void LoadTestCommandExecute()
		{
			try
			{
				var command = new LoadTestLogCommand();
				var argument = new TestCommandArgument(TestInfo);
				TestInformation testInformation = (TestInformation)ExecuteCommand(command, argument);
				if (null != TestInfo)
				{
					foreach (var testItem in TestInfo.TestItems)
					{
						var newTestItem = testInformation.TestItems
							.FirstOrDefault(_ => _.Equals(testItem));
						newTestItem.IsSelected = testItem.IsSelected;
					}
				}
				TestInfo = testInformation;

				UpdateCanCommandExecute();
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

		protected bool _isCheckAll = false;
		public bool IsCheckAll
		{
			get => _isCheckAll;
			set
			{
				_isCheckAll = value;
				foreach (TestItem item in TestInfo.TestItems)
				{
					item.IsSelected = value;
				}
				RaisePropertyChanged(nameof(IsCheckAll));
				ChangeTestSelectedByUserCommandExecute();
			}
		}
	}
}
