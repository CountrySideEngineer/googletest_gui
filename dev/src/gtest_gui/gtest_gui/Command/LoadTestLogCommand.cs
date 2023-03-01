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
		/// Output directory and file data.
		/// </summary>
		protected OutputDirAndFile _outputDirFile = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestLogCommand()
		{
			string currentDir = Directory.GetCurrentDirectory();
			_outputDirFile = new OutputDirAndFile(currentDir);
		}

		/// <summary>
		/// Execute command to load test log.
		/// </summary>
		/// <param name="cmdArgument">Command argument.</param>
		/// <returns>Collection of test file and TestCase object as test log in tuple.</returns>
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			TestInformation testInfo = cmdArgument.TestInfo;
			TestCase testCase = cmdArgument.TestCase;

			string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
			_outputDirFile.TestExeFileName = testFileName;
			_outputDirFile.TestTimeStamp = testCase.Timestamp;
			var reader = new TestLogReader()
			{
				OutputDirFile = _outputDirFile,
				TestCase = testCase
			};
			string content = reader.ReadTest(testInfo);
			string path = reader.GetLogFilePath();

			return (path, content);
		}
	}
}
