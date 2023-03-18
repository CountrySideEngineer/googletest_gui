using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.Command
{
	public class LoadTestHistoryCommand : ITestCommand
	{
		/// <summary>
		/// Output directory and file data.
		/// </summary>
		public OutputDirAndFile OutputDirFile { get; set; } = null;

		public TestListReader ListReader { get; set; } = null;

		public TestResultReader ResultReader { get; set; } = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestHistoryCommand()
		{
			string currentDir = Directory.GetCurrentDirectory();
			OutputDirFile = new OutputDirAndFile(currentDir);

			ListReader = new TestListReader();
			ResultReader = new TestResultReader();
		}

		/// <summary>
		/// Execute command to load test history data.
		/// </summary>
		/// <param name="cmdArgument">Argument for command.</param>
		/// <returns>Test history as a collection of TestCase object.</returns>
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			LoadTestHistoryCommandArgument arg = (LoadTestHistoryCommandArgument)cmdArgument;
			ResultReader.TargetPath = arg.TestPath;
			ResultReader.OutputDirFile = OutputDirFile;
			IEnumerable<TestCase> testCases = ResultReader.Read(arg.TestItems);
			IEnumerable<TestCase> cases = ExtractTestCase(arg.TestItems, testCases);

			return cases;
		}

		/// <summary>
		/// Extract test case data.
		/// </summary>
		/// <param name="items">Collection of test items.</param>
		/// <param name="cases">Collection of TestCase object to be extracted.</param>
		/// <returns>Extracted test case data.</returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		protected virtual IEnumerable<TestCase> ExtractTestCase(IEnumerable<TestItem> items, IEnumerable<TestCase> cases)
		{
			try
			{
				var testItem = items.First();
				var classAndTestName = testItem.Name.Split('.');
				string className = classAndTestName[0];
				string testName = classAndTestName[1];
				var testCases = cases.Where(_ =>
					_.Name.Equals(testName) && _.ClassName.Equals(className))
					.OrderByDescending(_ => _.Timestamp);

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
