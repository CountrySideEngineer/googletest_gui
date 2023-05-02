using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command
{
	public class LoadTestFromGoogleTestCommand : LoadTestCommand
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestFromGoogleTestCommand() : base()
		{
			ListReader = new GoogleTestListReader();
		}
	}
}
