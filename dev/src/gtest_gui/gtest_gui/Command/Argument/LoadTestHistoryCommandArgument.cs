using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
	public class LoadTestHistoryCommandArgument : TestCommandArgument
	{
		/// <summary>
		/// Path to test.
		/// </summary>
		public string TestPath { get; set; }

		/// <summary>
		/// Test item object to load that of history.
		/// </summary>
		public IEnumerable<TestItem> TestItems { get; set; }

		/// <summary>
		/// Default command argument.
		/// </summary>
		public LoadTestHistoryCommandArgument() : base() { }

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="testItems">Collection of TestItem object.</param>
		public LoadTestHistoryCommandArgument(IEnumerable<TestItem> testItems) :base()
		{
			TestItems = testItems;
		}
	}
}
