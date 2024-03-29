﻿using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using gtest_gui.Command.Exception;

namespace gtest_gui.Command
{
	/// <summary>
	/// Command class to get test log.
	/// </summary>
	public class LoadTestCommand : ITestCommand
	{
		/// <summary>
		/// Output directory and file data.
		/// </summary>
		public OutputDirAndFile OutputDirFile { get; set; } = null;

		/// <summary>
		/// Test list reader.
		/// </summary>
		public TestListReader ListReader { get; set; } = null;

		/// <summary>
		/// Test result reader.
		/// </summary>
		public TestResultReader ResultReader { get; set; } = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LoadTestCommand()
		{
			string currentDir = Directory.GetCurrentDirectory();
			OutputDirFile = new OutputDirAndFile(currentDir);

			ListReader = new TestListReader();
			ResultReader = new TestResultReader();
		}

		/// <summary>
		/// Execute command to read 
		/// </summary>
		/// <param name="cmdArgument">Argumetn for command.</param>
		/// <returns>Returs test log as <para>TestInformation</para> object.</returns>
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			try
			{
				string filePath = cmdArgument.TestInfo.TestFile;
				ListReader.TestFilePath = filePath;
				IEnumerable<TestItem> testItems = ListReader.Read();
				ResultReader.TargetPath = filePath;
				ResultReader.OutputDirFile = OutputDirFile;
				IEnumerable<TestCase> testCases = ResultReader.Read(testItems);
				SetResultToItem(testCases, testItems);

				return testItems;
			}
			catch (System.Exception ex)
			when (ex is NullReferenceException)
			{
				var exception = new CommandException(ex);
				throw exception;
			}
			catch (System.Exception ex)
			when (ex is ArgumentException)
			{
				var exception = new CommandException(ex);
				throw exception;
			}
			catch (System.Exception ex)
			when ((ex is OutOfMemoryException) || (ex is IOException))
			{
				var exception = new CommandException(ex);
				throw exception;
			}
		}

		protected virtual void SetResultToItem(IEnumerable<TestCase> cases, IEnumerable<TestItem> items)
		{
			foreach (var item in items)
			{
				try
				{
					string[] testCaseName = item.Name.Split('.');
					string className = testCaseName[0];
					string caseName = testCaseName[1];
					var testCase = cases.Where(_ =>
						_.Name.Equals(caseName) && _.ClassName.Equals(className))
						.OrderByDescending(_ => _.Timestamp)
						.FirstOrDefault();
					item.Result = testCase.Judge;
				}
				catch (NullReferenceException)
				{
					item.Result = string.Empty;
				}
			}
		}
	}
}
