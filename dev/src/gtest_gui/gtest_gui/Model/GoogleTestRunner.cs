using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace gtest_gui.Model
{
	public class GoogleTestRunner : TestRunner
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public GoogleTestRunner() : base() { }

		/// <summary>
		/// Get ProcessStartInformation object to run a test using google test framework.
		/// </summary>
		/// <param name="path">Path to test execution file.</param>
		/// <param name="item">Test item data.</param>
		/// <returns>ProcessStartInformation object to run test with default parameter.</returns>
		protected override ProcessStartInfo GetProcessStartInfo(string path, TestItem item)
		{
			string filterOption = GetFilterOption(path, item);
			string outputOption = GetXmlOutputOption(path, item);
			string option = $"{filterOption} {outputOption}";

			ProcessStartInfo procInfo = base.GetProcessStartInfo(path, item);
			procInfo.Arguments = option;

			return procInfo;
		}

		/// <summary>
		/// Create test filter option of google test framework.
		/// </summary>
		/// <param name="fileName">Test file path.</param>
		/// <param name="testItem">Test item information.</param>
		/// <returns>Test filter string.</returns>
		protected virtual string GetFilterOption(string fileName, TestItem testItem)
		{
			string filterOption = $"--gtest_filter={testItem.Name}";
			return filterOption;
		}

		/// <summary>
		/// Create test result output in XML format option.
		/// </summary>
		/// <param name="fileName">Test file path.</param>
		/// <param name="testItem">Test item information.</param>
		/// <returns>Test output filter option.</returns>
		protected virtual string GetXmlOutputOption(string fileName, TestItem testItem)
		{
			string xmlFilePath = OutputDirFile.TestReportFilePath(testItem.Name);
			string xmlOption = $"--gtest_output=xml:{xmlFilePath}";
			return xmlOption;
		}

	}
}
