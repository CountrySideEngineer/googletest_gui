using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest_gui.MoveWindow;
using gtest_gui.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command
{
	public class TestExecuteAsyncCommand : TestExecuteCommand
	{
		/// <summary>
		/// Execute test async.
		/// </summary>
		/// <param name="cmdArgument">Argument for test including target test file and information to run it.</param>
		/// <returns>Returns always 0.</returns>
		/// <remarks>This method will return contorl when the all test finished.</remarks>
		public override object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			TestRunner testRunner = base.SetUpTestRunner(cmdArgument);
			TestRunnerAsync testRunnerAsync = new TestRunnerAsync()
			{
				TestRunner = testRunner,
				TestInfo = cmdArgument.TestInfo
			};

			var viewModel = new ProgressWindowsViewModel()
			{
				AsyncTask = testRunnerAsync
			};
			var progress = new Progress<ProgressInfo>((_) =>
			{
				var cmdArg = new ProgressChangedCommandArgument()
				{
					ProgressInfo = _
				};
				viewModel.OnProgressChanged(this, cmdArg);

			});
			viewModel.Progress = progress;
			var view = new ProgressWindow()
			{
				DataContext = viewModel
			};
			view.ShowDialog();

			return null;
		}
	}
}
