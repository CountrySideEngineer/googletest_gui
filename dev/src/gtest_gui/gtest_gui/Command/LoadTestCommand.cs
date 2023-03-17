using gtest_gui.Command.Argument;
using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	/// <summary>
	/// Command class to get test log.
	/// </summary>
	public class LoadTestCommand : ITestCommand
	{
		/// <summary>
		/// Output directory and file data.
		/// </summary>
		public OutputDirAndFile OutputDirFile { get; set; } = null;

		public TestListReader ListReader { get; set; } = null;

		public TestResultReader ResultReader { get; set; } = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestCommand()
		{
			string currentDir = Directory.GetCurrentDirectory();
			OutputDirFile = new OutputDirAndFile(currentDir);

			ListReader = new TestListReader();

			ResultReader = new TestResultReader();
		}

		/// <summary>
		/// Execute command to read 
		/// </summary>
		/// <param name="cmdArgument">Argumetn for command.</param>
		/// <returns>Returs test log as <para>TestInformation</para> object.</returns>
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			string filePath = cmdArgument.TestInfo.TestFile;
			ListReader.TestFilePath = filePath;
			IEnumerable<TestItem> testItems = ListReader.Read();
			ResultReader.TargetPath = filePath;
			ResultReader.OutputDirFile = OutputDirFile;
			IEnumerable<TestItem> testResults = ResultReader.Read(testItems);

			var testInformation = new TestInformation()
			{
				TestItems = testResults
			};
			return testInformation;
		}
	}
}
