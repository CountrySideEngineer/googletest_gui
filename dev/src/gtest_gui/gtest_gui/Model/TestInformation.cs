using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Model
{
	public class TestInformation
	{
		/// <summary>
		/// Target test file.
		/// </summary>
		public string TestFile { get; set; }

		/// <summary>
		/// List of test items the test file specified by <para>TestFile</para> contains.
		/// </summary>
		public IEnumerable<TestItem> TestItems { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestInformation()
		{
			this.TestFile = string.Empty;
			var testItems = new List<TestItem>();
			testItems.Clear();

			this.TestItems = testItems;
		}
	}
}
