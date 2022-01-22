using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	public class LoadTestHistoryCommand : ITestCommand
	{
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			var testInfo = cmdArgument.TestInfo;
			string testFilenName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
			var outputDirFile = new OutputDirAndFile(Directory.GetCurrentDirectory(), testFilenName);
			var reader = new TestHistoryReader(testInfo.TestFile, outputDirFile);
			IEnumerable<TestCase> testCases = reader.ReadTest(testInfo);
			return testCases;
		}
	}
}
