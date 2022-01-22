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
	public class LoadTestLogCommand : ITestCommand
	{
		/// <summary>
		/// Execute command to read 
		/// </summary>
		/// <param name="cmdArgument">Argumetn for command.</param>
		/// <returns>Returs test log as <para>TestInformation</para> object.</returns>
		public object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			string filePath = cmdArgument.TestInfo.TestFile;
			var testRunner = new TestRunner()
			{
				Target = filePath
			};
			TestInformation testInformation = testRunner.GetTestList();
			string testExeFile = System.IO.Path.GetFileNameWithoutExtension(filePath);
			var outputDirFile = new OutputDirAndFile(Directory.GetCurrentDirectory(), testExeFile);
			var reader = new TestResultReader(filePath, outputDirFile);
			reader.ReadTest(testInformation);

			return testInformation;
		}
	}
}
