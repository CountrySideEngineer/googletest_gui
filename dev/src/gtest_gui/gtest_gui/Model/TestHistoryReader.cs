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


		public new IEnumerable<TestCase> ReadTest(TestInformation testInfo)
		{
			try
			{
				string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
				IEnumerable<string> testResultFiles = this.GetTestResultFiles(testFileName);
				IEnumerable<TestCase> srcTestCases = this.GetAllTestCases(testResultFiles);
				IEnumerable<TestCase> testCases = this.ExtractTestCase(testInfo, srcTestCases);

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

		protected virtual IEnumerable<TestCase> ExtractTestCase(TestInformation testInfo, IEnumerable<TestCase> srcCases)
		{
			try
			{
				var testItem = testInfo.TestItems.ElementAt(0);
				var classAndTestName = testItem.Name.Split('.');
				string className = classAndTestName[0];
				string testName = classAndTestName[1];
				var testCases = srcCases.Where(_ =>
					_.Name.Equals(testName) && _.ClassName.Equals(className));

				return testCases;
			}
			catch (Exception ex)
			when ((ex is IndexOutOfRangeException) || (ex is NullReferenceException))
			{
				throw;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				throw ex;
			}
		}
	}
}
 