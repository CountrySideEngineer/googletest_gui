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
		/// Path to file to execute tests.
		/// </summary>
		protected string _testFilePath;

		/// <summary>
		/// Test item of to handle in the view model.
		/// </summary>
		protected TestItem _testItem;

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
			_testCases = null;
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
				return _testCases;
			}
			set
			{
				_testCases = value;
				RaisePropertyChanged(nameof(this.TestCases));
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

		/// <summary>
		/// Name of test.
		/// </summary>
		public string TestName
		{
			get
			{
				try
				{
					return TestItem.Name;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// Path to test data file.
		/// </summary>
		public string TestFilePath
		{
			get
			{
				return _testFilePath;
			}
			set
			{
				_testFilePath = value;
				RaisePropertyChanged(nameof(TestFilePath));
			}
		}

		/// <summary>
		/// Displayed test tesm , TestItem object.
		/// </summary>
		public TestItem TestItem
		{
			get
			{
				return _testItem;
			}
			set
			{
				_testItem = value;
				RaisePropertyChanged(nameof(TestItem));
				RaisePropertyChanged(nameof(TestName));
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

		/// <summary>
		/// Load test history.
		/// </summary>
		public void LoadTestHistoryCommandExecute()
		{
			var testItems = new List<TestItem>()
			{
				TestItem
			};
			var commandArg = new LoadTestHistoryCommandArgument()
			{
				TestPath = TestFilePath,
				TestItems = testItems
			};
			var command = new LoadTestHistoryCommand();
			IEnumerable<TestCase> testCases = (IEnumerable<TestCase>)command.ExecuteCommand(commandArg);
			TestCases = testCases;
		}

		/// <summary>
		/// Show other window to show log content.
		/// </summary>
		public void ShowLogCommandExecute()
		{
			var mover = new Move2TestLog();
			mover.Move(this);
		}
	}
}
