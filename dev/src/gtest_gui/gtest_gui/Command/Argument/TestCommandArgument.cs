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
		/// Progress to notify test progress.
		/// </summary>
		public IProgress<TestInformation> Progress { get; set; }

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

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="progress">Progress object to notify progress of test.</param>
		public TestCommandArgument(IProgress<TestInformation> progress)
		{
			Progress = progress;
		}
	}
}
