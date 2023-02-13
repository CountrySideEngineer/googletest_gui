using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	public class LoadTestLogCommand : ITestCommand
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestLogCommand() { }

		/// <summary>
		/// Execute command to load test log.
		/// </summary>
		/// <param name="cmdArgument">Command argument.</param>
		/// <returns>Collection of test file and TestCase object as test log in tuple.</returns>
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			TestInformation testInfo = cmdArgument.TestInfo;
			string testFilenName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
			var outputDirFile = new OutputDirAndFile(Directory.GetCurrentDirectory(), testFilenName);
			var reader = new TestLogReader(testInfo.TestFile, outputDirFile);
			(IEnumerable<string>, IEnumerable<TestCase>) filesAndTestCases = reader.ReadTest(testInfo);

			return filesAndTestCases;
		}
	}
}
