using gtest_gui.Command.Argument;
using gtest_gui.Model;
using System;
using System.Collections.Generic;
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
			string filePath = cmdArgument.TargetFilePath;
			var testRunner = new TestRunner()
			{
				Target = filePath
			};
			TestInformation testInformation = testRunner.GetTestList();
			var reader = new TestResultReader();
			reader.ReadTest(testInformation);

			return testInformation;
		}
	}
}
