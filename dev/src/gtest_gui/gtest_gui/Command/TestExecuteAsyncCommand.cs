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

namespace gtest_gui.Command
{
	public class TestExecuteAsyncCommand : TestExecuteCommand
	{
		protected TestRunnerAsync _runnerAsync;

		protected CountrySideEngineer.ContentWindow.ContentWindow _contentWindow;

		protected CountrySideEngineer.ProgressWindow.ProgressWindow _proressWindow;

		public TestExecuteAsyncCommand() : base()
		{
			_runnerAsync = new TestRunnerAsync();
			_contentWindow = new CountrySideEngineer.ContentWindow.ContentWindow();
			_proressWindow = new CountrySideEngineer.ProgressWindow.ProgressWindow();
		}

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
			SetUpTestRunner(cmdArgument);

			string testFileName = System.IO.Path.GetFileNameWithoutExtension(cmdArgument.TestInfo.TestFile);
			_contentWindow.ViewTitle = testFileName;
			_contentWindow.Start();

			_proressWindow.Start(_runnerAsync);

			_contentWindow.Finish();

			TearDownTestRunner();

			return 0;
		}

		protected override void SetUpTestRunner(TestCommandArgument cmdArg)
		{
			base.SetUpTestRunner(cmdArg);

			_runner.TestDataReceivedEventHandler += _contentWindow.OnDataReceived;
			_runner.TestDataFinisedEventHandler += _contentWindow.OnDataRefresh;

			_runnerAsync.TestRunner = _runner;
			_runnerAsync.TestInfo = cmdArg.TestInfo;
		}

		protected override void TearDownTestRunner()
		{
			base.TearDownTestRunner();

			_runner.TestDataReceivedEventHandler -= _contentWindow.OnDataReceived;
			_runner.TestDataFinisedEventHandler -= _contentWindow.OnDataRefresh;
		}
	}
}
