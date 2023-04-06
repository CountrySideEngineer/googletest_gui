using gtest_gui.Command.Argument;
using gtest_gui.Command.Exception;
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
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			try
			{
				var testLogArg = (LoadTestLogCommandArgument)cmdArgument;
				string testPath = testLogArg.TestPath;
				TestCase testCase = testLogArg.TestCase;
				string testFileName = Path.GetFileNameWithoutExtension(testPath);
				_outputDirFile.TestExeFileName = testFileName;
				_outputDirFile.TestTimeStamp = testCase.Timestamp;
				var reader = new TestLogReader()
				{
					OutputDirFile = _outputDirFile
				};
				string content = reader.Read(testCase);

				return content;
			}
			catch (InvalidCastException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			catch (NullReferenceException ex)
			{
				throw new ArgumentNullException(ex.Message);
			}
			catch (System.Exception ex)
			when ((ex is ArgumentException) ||
				(ex is FileNotFoundException) || 
				(ex is InvalidOperationException) ||
				(ex is OutOfMemoryException) ||
				(ex is IOException))
			{
				throw new CommandException(ex);
			}
		}
	}
}
