using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.EventArg
{
	public class CommandFinishedEventArgs : EventArgs
	{
		/// <summary>
		/// Command Title;
		/// </summary>
		public string CommandTitile { get; set; }

		/// <summary>
		/// Command result message.
		/// </summary>
		public string ResultMessage { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CommandFinishedEventArgs() : base()
		{
			CommandTitile = string.Empty;
			ResultMessage = string.Empty;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="title">Executed command name.</param>
		/// <param name="message">Command result message.</param>
		public CommandFinishedEventArgs(string title, string message)
			: base()
		{
			CommandTitile = title;
			ResultMessage = message;
		}
	}
}
