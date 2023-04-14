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
		/// Title of error.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Error summary.
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// Default exception
		/// </summary>
		public CommandException() : base()
		{
			Code = 0;
			Title = string.Empty;
			Summary = string.Empty;	
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
