using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Model
{
	public class TestDataFinishedEventArgs : EventArgs
	{
		/// <summary>
		/// Default constructor 
		/// </summary>
		public TestDataFinishedEventArgs() { }

		/// <summary>
		/// Finished test item.
		/// </summary>
		public TestItem TestItem { get; set; }
	}
}
