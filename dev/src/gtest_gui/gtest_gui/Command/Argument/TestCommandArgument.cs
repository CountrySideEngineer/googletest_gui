using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class TestCommandArgument
	{
		/// <summary>
		/// Test information data.
		/// </summary>
		public TestInformation TestInfo { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestCommandArgument()
		{
			this.TestInfo = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="targetFile">Path to file to execute test.</param>
		/// <param name="testInfo">Test informations.</param>
		public TestCommandArgument(TestInformation testInfo)
		{
			this.TestInfo = testInfo;
		}
	}
}
