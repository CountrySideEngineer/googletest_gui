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
			TestFile = string.Empty;
			var testItems = new List<TestItem>();
			testItems.Clear();

			TestItems = testItems;
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="src"></param>
		public TestInformation(TestInformation src)
		{
			TestFile = src.TestFile;
			TestItems = new List<TestItem>(src.TestItems);
		}

		/// <summary>
		/// Compare with other TestInformation object.
		/// </summary>
		/// <param name="target">Object to compare.</param>
		/// <returns>If the target equals this, retunrs true, otherwiese false.</returns>
		public bool Equals(TestInformation target)
		{
			try
			{
				if (TestFile == target.TestFile)
				{

					return true;
				}
				else
				{
					return false;
				}
			}
			catch (NullReferenceException)
			{
				return false;
			}
		}
	}
}
