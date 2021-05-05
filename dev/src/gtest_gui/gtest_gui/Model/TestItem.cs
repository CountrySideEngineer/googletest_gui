using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Model
{
	public class TestItem
	{
		/// <summary>
		/// Test name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Get and set the value indicates whether the test item is selected to run or not.
		/// </summary>
		public bool IsSelected { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestItem()
		{
			this.Name = string.Empty;
			this.IsSelected = false;
		}
	}
}
