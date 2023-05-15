using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest_gui.MoveWindow;
using gtest_gui.View;
using System;
using System.Collections.Generic;
using System.Text;
using CountrySideEngineer.ProgressWindow;
using CountrySideEngineer.ProgressWindow.Model;
using CountrySideEngineer.ProgressWindow.ViewModel;
using System.Diagnostics;
using gtest_gui.Command.Exception;

namespace gtest_gui.Command
{
	public class TestExecuteAsyncCommand : TestExecuteCommand
	{
		/// <summary>
		/// Run test async.
		/// </summary>
		protected TestRunnerAsync _runnerAsync;

		/// <summary>
		/// Content window class.
		/// </summary>
		protected CountrySideEngineer.ContentWindow.ContentWindow _contentWindow;

		/// <summary>
		/// Progress window class.
		/// </summary>
		protected ProgressWindow _proressWindow;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestExecuteAsyncCommand() : base()
		{
			_runnerAsync = new TestRunnerAsync();
			_contentWindow = new CountrySideEngineer.ContentWindow.ContentWindow();
			_proressWindow = new CountrySideEngineer.ProgressWindow.ProgressWindow();
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="runner"></param>
		public TestExecuteAsyncCommand(TestRunner runner) : base(runner)
		{
			_runnerAsync = new TestRunnerAsync();
			_contentWindow = new CountrySideEngineer.ContentWindow.ContentWindow();
			_proressWindow = new CountrySideEngineer.ProgressWindow.ProgressWindow();
		}

		/// <summary>
		/// Execute test async.
		/// </summary>
		/// <param name="cmdArgument">Argument for test including target test file and information to run it.</param>
		/// <returns>Returns always 0.</returns>
		/// <remarks>This method will return contorl when the all test finished.</remarks>
		public override object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			try
			{
				SetUpTestRunner(cmdArgument);

				_contentWindow.ViewTitle = System.IO.Path.GetFileNameWithoutExtension(cmdArgument.TestInfo.TestFile);
				_contentWindow.Start();

				_proressWindow.Start(_runnerAsync);

				_contentWindow.Finish();

				TearDownTestRunner();

				return 0;
			}
			catch (System.Exception ex)
			when ((ex is ArgumentException) ||
				(ex is UnauthorizedAccessException) ||
				(ex is NotSupportedException))
			{
				var exception = new CommandException()
				{
					Code = 0x00000001,
					Title = "テスト実行エラー",
					Summary = "選択されたテストが実行できませんでした。" + Environment.NewLine +
						"実行ファイルの有無、指定されたテストがGoogletestを使用しているか、確認してください。"
				};
				throw exception;
			}
			catch (System.Exception ex)
			when (ex is NullReferenceException)
			{
				throw;
			} 
		}

		/// <summary>
		/// Set up TestRunner object with parameters passed with argument, TestCommandArgument.
		/// </summary>
		/// <param name="cmdArg">Command argument used to setup TestRunner object.</param>
		/// <returns>TestRunner object to run test.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		protected override void SetUpTestRunner(TestCommandArgument cmdArg)
		{
			base.SetUpTestRunner(cmdArg);

			_runner.TestDataReceivedEventHandler += _contentWindow.OnDataReceived;
			_runner.TestDataFinisedEventHandler += _contentWindow.OnDataRefresh;

			_runnerAsync.TestRunner = _runner;
			_runnerAsync.TestInfo = cmdArg.TestInfo;
		}

		/// <summary>
		/// Teardown the test.
		/// </summary>
		protected override void TearDownTestRunner()
		{
			base.TearDownTestRunner();

			_runner.TestDataReceivedEventHandler -= _contentWindow.OnDataReceived;
			_runner.TestDataFinisedEventHandler -= _contentWindow.OnDataRefresh;
		}
	}
}
