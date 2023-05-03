using gtest2html;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class LoadTestLogCommandArgument : TestCommandArgument
	{
		/// <summary>
		/// Test path.
		/// </summary>
		public string TestPath { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestLogCommandArgument() : base() { }
	}
}
