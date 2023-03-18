using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.Model
{
	public class TestLogReader : TestHistoryReader
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestLogReader() : base() { }

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="target">Test execution file path.</param>
		/// <param name="outputDirAndFile"><para>OutputDirAndFile</para> object.</param>
		/// <remarks>If the <para>outputDirFile</para> is not set, null will be passed.</remarks>
		public TestLogReader(string target, OutputDirAndFile outputDirAndFile = null)
			: base(target, outputDirAndFile)
		{ }

		public virtual string Read(TestCase testCase)
		{
			try
			{
				string path = GetLogFilePath(testCase);
				string content = GetLog(path);

				return content;
			}
			catch (DirectoryNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
			{
				throw;
			}
		}

		public virtual string GetLogFilePath(TestCase testCase)
		{
			string path = OutputDirFile.LogFilePath(testCase);

			return path;
		}

		protected virtual string GetLog(string path)
		{
			using (var reader = new StreamReader(path))
			{
				string content = reader.ReadToEnd();

				return content;
			}
		}


		/// <summary>
		/// Extract test file path of target test.
		/// </summary>
		/// <param name="testInfo">Test information.</param>
		/// <param name="testResultFiles">Collectoin of all test log files.</param>
		/// <returns>Collection of test log file of target test file.</returns>
		protected virtual IEnumerable<string> ExtractTestFile(TestInformation testInfo, IEnumerable<string> testResultFiles)
		{
			try
			{
				TestItem testItem = testInfo.TestItems.First();
				var testFiels = testResultFiles.Where(_ => _.Contains(testItem.Name));
				return testFiels;
			}
			catch (Exception ex)
			when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
			{
				throw;
			}
		}
	}
}
