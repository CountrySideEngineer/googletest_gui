using CountrySideEngineer.ViewModel.Base;
using gtest_gui.Command;
using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.ViewModel
{
	public class TestLogViewModel : ViewModelBase
	{

		/// <summary>
		/// Test case date to handle in this view model.
		/// </summary>
		protected TestCase _testCase;

		/// <summary>
		/// Field of window title.
		/// </summary>
		protected string _windowTitle;

		/// <summary>
		/// Logs loaded from file specified by _logFilePath;
		/// </summary>
		protected string _logContent;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestLogViewModel()
		{
			_testCase = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="testCase"></param>
		public TestLogViewModel(TestCase testCase)
		{
			_testCase = testCase;
		}

		/// <summary>
		/// Test information.
		/// </summary>
		public TestInformation TestInformation { get; set; }

		/// <summary>
		/// Path to file to show in the content area.
		/// </summary>
		public string LogFilePath { get; set; }

		/// <summary>
		/// Window title property.
		/// </summary>
		public string WindowTitle
		{
			get
			{
				return LogFilePath;
			}
			set
			{
				LogFilePath = value;
				RaisePropertyChanged(nameof(WindowTitle));
			}
		}

		/// <summary>
		/// Log content property.
		/// </summary>
		public string LogContent
		{
			get
			{
				return _logContent;
			}
			set
			{
				_logContent = value;
				RaisePropertyChanged(nameof(LogContent));
			}
		}

		/// <summary>
		/// Load log content from file.
		/// </summary>
		public void LoadTestLogCommandExecute()
		{
			var command = new LoadTestLogCommand();
			var commandArg = new TestCommandArgument()
			{
				TestInfo = TestInformation,
				TestCase = _testCase
			};
			string content = (string)command.ExecuteCommand(commandArg);
			LogContent = content;
		}

		/// <summary>
		/// Get log file path to load.
		/// </summary>
		public void GetTestLogFilePathCommandExecute()
		{
			var command = new GetLogFilePathCommand();
			var commandArg = new TestCommandArgument()
			{
				TestInfo = TestInformation,
				TestCase = _testCase
			};
			string filePath = (string)command.ExecuteCommand(commandArg);
			string fileName = Path.GetFileName(filePath);
			WindowTitle = fileName;
		}
	}
}
