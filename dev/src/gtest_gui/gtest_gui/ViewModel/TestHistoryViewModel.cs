using gtest_gui.Command;
using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountrySideEngineer.ViewModel.Base;
using System.IO;
using gtest_gui.MoveWindow;

namespace gtest_gui.ViewModel
{
	public class TestHistoryViewModel : ViewModelBase
	{
		/// <summary>
		/// Field of test cases.
		/// </summary>
		protected IEnumerable<TestCase> _testCases;

		/// <summary>
		/// Field of collection of test log file.
		/// </summary>
		protected IEnumerable<string> _testLogFiles;

		/// <summary>
		/// Test name.
		/// </summary>
		protected TestInformation _testName;

		/// <summary>
		/// Test information.
		/// </summary>
		protected TestInformation _testInformation;

		/// <summary>
		/// Current selected test index.
		/// </summary>
		protected int _selectedIndex;

		/// <summary>
		/// Field of command to show test log.
		/// </summary>
		protected DelegateCommand _showLogCommand;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestHistoryViewModel()
		{
			this._testCases = null;
		}

		/// <summary>
		/// Current selected test index property.
		/// </summary>
		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				_selectedIndex = value;
				RaisePropertyChanged(nameof(SelectedIndex));
			}
		}

		/// <summary>
		/// List of test case, TestCase object.
		/// </summary>
		public IEnumerable<TestCase> TestCases
		{
			get
			{
				return this._testCases;
			}
			set
			{
				this._testCases = value;
				this.RaisePropertyChanged(nameof(this.TestCases));
			}
		}

		/// <summary>
		/// Properrt of collection of test log files.
		/// </summary>
		public IEnumerable<string> TestLogFiles
		{
			get
			{
				return _testLogFiles;
			}
			set
			{
				_testLogFiles = value;
				RaisePropertyChanged(nameof(TestLogFiles));
			}
		}

		public string TestName
		{
			get
			{
				try
				{
					return this.TestInformation.TestItems.ElementAt(0).Name;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}

		public TestInformation TestInformation
		{
			get
			{
				return this._testInformation;
			}
			set
			{
				this._testInformation = value;
				this.RaisePropertyChanged(nameof(TestInformation));
				this.RaisePropertyChanged(nameof(TestName));
			}
		}

		/// <summary>
		/// Delegate command of show log dialog.
		/// </summary>
		public DelegateCommand ShowLogCommand
		{
			get
			{
				if (null == _showLogCommand)
				{
					_showLogCommand = new DelegateCommand(ShowLogCommandExecute);
				}
				return _showLogCommand;
			}
		}

		public void LoadTestHistoryCommandExecute()
		{
			var commandArg = new TestCommandArgument(this.TestInformation);
			var command = new LoadTestHistoryCommand();
			IEnumerable<TestCase> testCases = (IEnumerable<TestCase>)command.ExecuteCommand(commandArg);
			List<TestCase> testCaseList = testCases.ToList();
			this.TestCases = testCaseList;
		}

		/// <summary>
		/// Show other window to show log content.
		/// </summary>
		public void ShowLogCommandExecute()
		{
			var commandArg = new TestCommandArgument(TestInformation);
			var command = new LoadTestLogCommand();
			IEnumerable<string> files = (IEnumerable<string>)command.ExecuteCommand(commandArg);
			string file = files.ElementAt(SelectedIndex);

			var mover = new Move2TestLog()
			{
				LogFilePath = file
			};
			mover.Move(this);
		}
	}
}
