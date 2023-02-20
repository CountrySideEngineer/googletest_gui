using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.Model
{
	public class TestHistoryReader : TestResultReader
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestHistoryReader() : base() { }

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="target">Test execution file path.</param>
		/// <param name="outputDirFile"><para>OutputDirAndFile</para> object.</param>
		/// <remarks>If the <para>outputDirFile</para> is not set, null will be passed.</remarks>
		public TestHistoryReader(string target, OutputDirAndFile outputDirFile = null) 
			: base(target, outputDirFile)
		{}

		/// <summary>
		/// Returns collection of tests.
		/// </summary>
		/// <param name="testInfo">Test information.</param>
		/// <returns>Collection of tests.</returns>
		/// <exception cref="DirectoryNotFoundException"></exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public new virtual IEnumerable<TestCase> ReadTest(TestInformation testInfo)
		{
			try
			{
				string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
				IEnumerable<string> testResultFiles = GetTestResultFiles(testFileName);
				IEnumerable<TestCase> srcTestCases = GetAllTestCases(testResultFiles);
				IEnumerable<TestCase> testCases = ExtractTestCase(testInfo, srcTestCases);

				return testCases;
			}
			catch (DirectoryNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			when ((ex is IndexOutOfRangeException) || (ex is NullReferenceException))
			{
				return null;
			}
		}

		/// <summary>
		/// Extract test case data.
		/// </summary>
		/// <param name="testInfo">Test information.</param>
		/// <param name="srcCases">Collection of TestCase object to be extractd.</param>
		/// <returns>Extracted test case data.</returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		protected virtual IEnumerable<TestCase> ExtractTestCase(TestInformation testInfo, IEnumerable<TestCase> srcCases)
		{
			try
			{
				var testItem = testInfo.TestItems.First();
				var classAndTestName = testItem.Name.Split('.');
				string className = classAndTestName[0];
				string testName = classAndTestName[1];
				var testCases = srcCases.Where(_ =>
					_.Name.Equals(testName) && _.ClassName.Equals(className));

				return testCases;
			}
			catch (Exception ex)
			when ((ex is IndexOutOfRangeException) || (ex is InvalidOperationException))
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
 