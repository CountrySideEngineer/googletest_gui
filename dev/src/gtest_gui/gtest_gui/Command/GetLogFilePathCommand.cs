using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	public class GetLogFilePathCommand : ITestCommand
	{
		protected OutputDirAndFile _outptuDirFile = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GetLogFilePathCommand()
		{
			string currentDir = Directory.GetCurrentDirectory();
			_outptuDirFile = new OutputDirAndFile(currentDir);
		}

		/// <summary>
		/// Execute command to get log file path.
		/// </summary>
		/// <param name="cmdArgument">Command argument.</param>
		/// <returns>Path to log specified by test case and test execution ilfe.</returns>
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			TestCase testCase = cmdArgument.TestCase;
			string testFilePath = cmdArgument.TestInfo.TestFile;
			string testFileName = Path.GetFileNameWithoutExtension(testFilePath);

			_outptuDirFile.TestExeFileName = testFileName;
			_outptuDirFile.TestTimeStamp = testCase.Timestamp;
			var reader = new TestLogReader()
			{
				OutputDirFile = _outptuDirFile,
				TestCase = testCase
			};
			string logFilePath = reader.GetLogFilePath();

			return logFilePath;
		}
	}
}
