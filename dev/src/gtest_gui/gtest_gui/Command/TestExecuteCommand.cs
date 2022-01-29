using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest_gui.MoveWindow;
using gtest_gui.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	/// <summary>
	/// Command class to execute test
	/// </summary>
	public class TestExecuteCommand : ITestCommand
	{
		/// <summary>
		/// Execute test.
		/// </summary>
		/// <param name="cmdArgument">Argument for test including target test file and information to run it.</param>
		/// <returns>Returns always 0.</returns>
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			string testFilePath = cmdArgument.TestInfo.TestFile;
			string testFileName = System.IO.Path.GetFileNameWithoutExtension(testFilePath);
			var outputDirInfo = new OutputDirAndFile(Directory.GetCurrentDirectory(), testFileName);
			TestInformation testInformation = cmdArgument.TestInfo;
			var testRunner = new TestRunner
			{
				Target = testFilePath,
				OutputDirFile = outputDirInfo
			};
			var outputLogBuilder = new OutputLogBuilder(outputDirInfo);
			testRunner.TestDataReceivedEventHandler += outputLogBuilder.OnDataReceived;
			testRunner.TestDataFinisedEventHandler += outputLogBuilder.OnDataReceiveFinished;

			var viewModel = new ProgressWindowsViewModel();
			var progress = new Progress<ProgressInfo>(this.OnProgressChanged);

			var dataContext = new ProgressWindowsViewModel();
			ProgressChangedEventHandler += dataContext.OnProgressChanged;
			var window = new ProgressWindow()
			{
				DataContext = dataContext
			};
			testRunner.RunAsync(testInformation, progress);
			window.ShowDialog();

			return (int)0;
		}

		public delegate void ProgressChangedEvent(object sender, ProgressChangedCommandArgument e);
		public ProgressChangedEvent ProgressChangedEventHandler;

		public void OnProgressChanged(ProgressInfo progressInfo)
		{
			Debug.WriteLine($"      Title = {progressInfo.Title}");
			Debug.WriteLine($"       Name = {progressInfo.ProcessName}");
			Debug.WriteLine($"   Progress = {progressInfo.Progress}");
			Debug.WriteLine($"  Numerator = {progressInfo.Numerator}");
			Debug.WriteLine($"Denominator = {progressInfo.Denominator}");

			ProgressChangedEventHandler?.Invoke(this,
				new ProgressChangedCommandArgument
				{
					ProgressInfo = progressInfo
				});
		}
	}
}
