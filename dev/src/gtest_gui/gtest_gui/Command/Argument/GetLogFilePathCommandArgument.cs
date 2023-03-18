using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class GetLogFilePathCommandArgument : TestCommandArgument
	{
		/// <summary>
		/// Path to file to test execute.
		/// </summary>
		public string TestPath { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public GetLogFilePathCommandArgument() : base() { }
	}
}
