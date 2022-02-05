using gtest_gui.Model;
using CountrySideEngineer.ProgressWindow.Model;
using CountrySideEngineer.ProgressWindow.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class ProgressChangedCommandArgument
	{
		/// <summary>
		/// Progress information to update.
		/// </summary>
		public ProgressInfo ProgressInfo { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ProgressChangedCommandArgument()
		{
			ProgressInfo = new ProgressInfo();
		}
	}
}
