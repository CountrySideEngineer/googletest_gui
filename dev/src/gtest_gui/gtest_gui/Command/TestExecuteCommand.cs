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
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			TestRunner testRunner = SetUpTestRunner(cmdArgument);
			TestInformation testInfo = cmdArgument.TestInfo;
			testRunner.Run(testInfo);

			return 0;
		}

		/// <summary>
		/// Set up TestRunner object with parameters passed with argument, TestCommandArgument.
		/// </summary>
		/// <param name="cmdArg">Command argument used to setup TestRunner object.</param>
		/// <returns>TestRunner object to run test.</returns>
		protected virtual TestRunner SetUpTestRunner(TestCommandArgument cmdArg)
		{
			string testFilePath = cmdArg.TestInfo.TestFile;
			string testFileName = System.IO.Path.GetFileNameWithoutExtension(testFilePath);
			var outputDirInfo = new OutputDirAndFile(Directory.GetCurrentDirectory(), testFileName);
			var testRunner = new TestRunner
			{
				Target = testFilePath,
				OutputDirFile = outputDirInfo
			};
			var outputLogBuilder = new OutputLogBuilder(outputDirInfo);
			testRunner.TestDataReceivedEventHandler += outputLogBuilder.OnDataReceived;
			testRunner.TestDataFinisedEventHandler += outputLogBuilder.OnDataReceiveFinished;

			return testRunner;
		}
	}
}
