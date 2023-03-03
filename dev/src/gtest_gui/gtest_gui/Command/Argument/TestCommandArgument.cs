using gtest_gui.Model;
using gtest2html;
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
		/// Test case data.
		/// </summary>
		public TestCase TestCase { get; set; }

		/// <summary>
		/// Progress to notify test progress.
		/// </summary>
		public IProgress<TestInformation> Progress { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestCommandArgument()
		{
			TestInfo = null;
			TestCase = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="testInfo">Test information.</param>
		public TestCommandArgument(TestInformation testInfo)
		{
			TestInfo = testInfo;
			TestCase = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="testCase">Test case data.</param>
		public TestCommandArgument(TestCase testCase)
		{
			TestInfo = null;
			TestCase = testCase;
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
