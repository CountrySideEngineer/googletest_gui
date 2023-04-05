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

		/// <summary>
		/// Read test log data.
		/// </summary>
		/// <param name="testCase">Test case data to read.</param>
		/// <returns>Read log datga.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="OutOfMemoryException"></exception>
		/// <exception cref="IOException"></exception>
		public virtual string Read(TestCase testCase)
		{
			try
			{
				string path = GetLogFilePath(testCase);
				string content = GetLog(path);

				return content;
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (ArgumentNullException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			catch (Exception ex)
			when ((ex is ArgumentNullException) || (ex is InvalidOperationException))
			{
				throw;
			}
		}

		/// <summary>
		/// Get log file path.
		/// </summary>
		/// <param name="testCase">Test case information.</param>
		/// <returns>Log file path.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public virtual string GetLogFilePath(TestCase testCase)
		{
			try
			{
				string path = OutputDirFile.LogFilePath(testCase);

				return path;
			}
			catch (ArgumentNullException)
			{
				throw;
			}
		}

		/// <summary>
		/// Get log from path
		/// </summary>
		/// <param name="path">Path to log data.</param>
		/// <returns>Log data.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="OutOfMemoryException"></exception>
		protected virtual string GetLog(string path)
		{
			try
			{
				using (var reader = new StreamReader(path))
				{
					string content = reader.ReadToEnd();

					return content;
				}
			}
			catch (System.Exception ex)
			when ((ex is ArgumentException) || (ex is ArgumentNullException))
			{
				throw new ArgumentException(string.Empty, ex);
			}
			catch (System.Exception ex)
			when ((ex is FileNotFoundException) || (ex is DirectoryNotFoundException))
			{
				throw new FileNotFoundException(string.Empty, ex);
			}
			catch (System.Exception ex)
			when ((ex is IOException) || (ex is OutOfMemoryException))
			{
				throw;
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
