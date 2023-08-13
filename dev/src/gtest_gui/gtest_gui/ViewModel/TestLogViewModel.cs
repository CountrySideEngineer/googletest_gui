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
	public class TestLogViewModel : GTestGuiViewModelBase
	{
		/// <summary>
		/// Path to file to execute tests.
		/// </summary>
		protected string _testFilePath;

		/// <summary>
		/// Log file path.
		/// </summary>
		protected string _logFilePath;

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
			LogFilePath = string.Empty;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="testCase"></param>
		public TestLogViewModel(string testFilePath, TestCase testCase)
		{
			_testFilePath = testFilePath;
			_testCase = testCase;
		}

		/// <summary>
		/// Path to file to show in the content area.
		/// </summary>
		public string LogFilePath
		{
			get => _logFilePath;
			set
			{
				_logFilePath = value;
				RaisePropertyChanged(nameof(LogFilePath));
				RaisePropertyChanged(nameof(WindowTitle));
			}
		}

		/// <summary>
		/// Window title property.
		/// </summary>
		public string WindowTitle
		{
			get
			{
				string windowTitle = "実行ログ";
				if ((!string.IsNullOrEmpty(LogFilePath)) && (!string.IsNullOrWhiteSpace(LogFilePath)))
				{
					windowTitle += " - " + LogFilePath;
				}
				return windowTitle;
			}
		}

		/// <summary>
		/// Log content property.
		/// </summary>
		public string LogContent
		{
			get => _logContent;
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
			try
			{
				var command = new LoadTestLogCommand();
				var commandArg = new LoadTestLogCommandArgument
				{
					TestPath = _testFilePath,
					TestCase = _testCase
				};
				string content = (string)command.ExecuteCommand(commandArg);
				LogContent = content;
			}
			catch (Exception)
			{
				NotifyError(null);
			}
		}

		/// <summary>
		/// Get log file path to load.
		/// </summary>
		public void GetTestLogFilePathCommandExecute()
		{
			var command = new GetLogFilePathCommand();
			var commandArg = new GetLogFilePathCommandArgument()
			{
				TestPath = _testFilePath,
				TestCase = _testCase
			};
			string filePath = (string)command.ExecuteCommand(commandArg);
			LogFilePath = filePath;
		}
	}
}
