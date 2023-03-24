using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Exception
{
	public class CommandException : System.Exception
	{
		/// <summary>
		/// Default exception
		/// </summary>
		public CommandException() : base() { }

		public CommandException(System.Exception innerException) : base(string.Empty, innerException) { }
	}
}
