using gtest_gui.Command.Argument;
using gtest_gui.Model;
using System;
using System.Collections.Generic;
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
			string testFilePath = cmdArgument.TargetFilePath;
			string testFileName = System.IO.Path.GetFileNameWithoutExtension(testFilePath);
			var outputDirInfo = new OutputDirAndFile(Directory.GetCurrentDirectory(), testFileName);
			TestInformation testInformation = cmdArgument.TestInfo;
			var testRunner = new TestRunner
			{
				Target = testFilePath,
				OutputDirFile = outputDirInfo
			};
			testRunner.Run(testInformation);
			return (int)0;
		}
	}
}
