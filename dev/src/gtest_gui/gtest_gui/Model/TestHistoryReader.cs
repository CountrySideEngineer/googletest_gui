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

		public new virtual IEnumerable<TestCase> Read(IEnumerable<TestItem> testItems)
		{
			return null;
		}

		/// <summary>
		/// Returns collection of tests.
		/// </summary>
		/// <param name="testInfo">Test information.</param>
		/// <returns>Collection of tests.</returns>
		/// <exception cref="DirectoryNotFoundException"></exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		public virtual IEnumerable<TestCase> ReadTest(TestInformation testInfo)
		{
			try
			{
				string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
				IEnumerable<string> testResultFiles = GetTestResultFiles(testFileName);
				IEnumerable<TestCase> testCases = GetAllTestCases(testResultFiles);
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
	}
}
