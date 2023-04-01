using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Exception
{
	public class CommandException : System.Exception
	{
		/// <summary>
		/// Error code about exception.
		/// </summary>
		public ulong Code { get; set; }

		/// <summary>
		/// Default exception
		/// </summary>
		public CommandException() : base()
		{
			Code = 0;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="innerException"></param>
		public CommandException(System.Exception innerException) : base(string.Empty, innerException)
		{
			Code = 0;
		}
	}
}
