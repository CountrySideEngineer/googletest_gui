using gtest_gui.Command;
using gtest_gui.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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
		/// Field of which a test can run or not.
		/// </summary>
		protected bool _canRunTest;

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

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GTestGuiViewModel()
		{
			this.TestFilePath = string.Empty;
			this.CanRunTest = false;
		}

		/// <summary>
		/// Test file path.
		/// </summary>
		public string TestFilePath
		{
			get
			{
				return this._testFilePath;
			}
			set
			{
				this._testFilePath = value;
				this.RaisePropertyChanged("TestFilePath");
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
				this.RaisePropertyChanged("TestInfo");
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
				this.TestFilePath = dialog.FileName;

				var runner = new TestRunner();
				TestInformation testInfo = runner.GetTestList(this.TestFilePath);
				this.TestInfo = testInfo;
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

		public void RunTestCommandExecute()
		{
			var runner = new TestRunner
			{
				Target = this.TestFilePath
			};
			runner.Run(this.TestInfo);
		}
	}
}
