using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class TestCommandArgument
	{
		/// <summary>
		/// Test execute file path.
		/// </summary>
		public string TargetFilePath { get; protected set; }

		/// <summary>
		/// Test information data.
		/// </summary>
		public TestInformation TestInfo { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestCommandArgument()
		{
			this.TargetFilePath = string.Empty;
			this.TestInfo = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="targetFile">Path to file to execute test.</param>
		/// <param name="testInfo">Test informations.</param>
		public TestCommandArgument(string targetFile, TestInformation testInfo = null)
		{
			this.TargetFilePath = targetFile;
			this.TestInfo = testInfo;
		}
	}
}
