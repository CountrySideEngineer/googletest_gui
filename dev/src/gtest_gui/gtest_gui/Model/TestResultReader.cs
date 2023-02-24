﻿using gtest2html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace gtest_gui.Model
{
	public class TestResultReader
	{
		/// <summary>
		/// Path to test execution file.
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// The log and file information.
		/// </summary>
		public OutputDirAndFile OutputDirFile { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestResultReader()
		{
			Target = string.Empty;
			OutputDirFile = null;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="target">Test target file path.</param>
		/// <param name="outputDirFile">OutputDirAndFile object</param>
		/// <remarks>If the <para>outputDirFile</para> is not set, null will be passed.</remarks>
		public TestResultReader(string target, OutputDirAndFile outputDirFile = null)
		{
			Target = target;
			OutputDirFile = outputDirFile;
		}

		/// <summary>
		/// Read test data corresponding to the test execution file.
		/// </summary>
		/// <param name="testInfo">Test information file includes the test execution file.</param>
		public virtual void ReadTest(TestInformation testInfo)
		{
			try
			{
				string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
				IEnumerable<string> testResultFiles = GetTestResultFiles(testFileName);
				IEnumerable<TestCase> testCases = GetAllTestCases(testResultFiles);
				SetTestResult(testInfo, testCases);
			}
			catch (DirectoryNotFoundException)
			{
				/*
				 *	The test has never been run.
				 *	Skip the operation.
				 */
			}
		}

		/// <summary>
		/// Get collection of test resutl file path corresponding to the test execution file specified by <para>fileName</para>.
		/// </summary>
		/// <param name="fileName">Test execution file path.</param>
		/// <returns>Collection of test result file path.</returns>
		/// <exception cref="DirectoryNotFoundException">The report output directory can not found.</exception>
		protected virtual IEnumerable<string> GetTestResultFiles(string fileName)
		{
			try
			{
				IEnumerable<string> reportFiles = OutputDirFile.GetTestReportFiles();
				return reportFiles;
			}
			catch (DirectoryNotFoundException)
			{
				throw;
			}
		}

		/// <summary>
		/// Get <para>TestSuites</para> objects from files <para>testFiles</para>.
		/// </summary>
		/// <param name="testFiles">Collection of test result file path.</param>
		/// <returns>Colelction of <para>TestSuites</para> read from files.</returns>
		protected virtual IEnumerable<TestSuites> GetTestSuites(IEnumerable<string> testFiles)
		{
			var testSuitesList = new List<TestSuites>();
			foreach (var testFile in testFiles)
			{
				var testSuites = GetTestSuites(testFile);
				testSuitesList.Add(testSuites);
			}
			return testSuitesList;
		}

		/// <summary>
		/// Extract <para>TestSuite</para> objects from collection of <para>TestSuites</para>.
		/// </summary>
		/// <param name="testSuitesList">Collection of <para>TestSuites</para> object.</param>
		/// <returns>Collection of <para>TestSuite</para>.</returns>
		protected virtual IEnumerable<TestSuite> GetTestSuite(IEnumerable<TestSuites> testSuitesList)
		{
			var testSuiteList = new List<TestSuite>();
			foreach (var testSuitesItem in testSuitesList)
			{
				var testSuites = testSuitesItem.TestItems;
				testSuiteList = testSuiteList.Union(testSuites).ToList();
			}
			return testSuiteList;
		}

		/// <summary>
		/// Get <para>TestSuites</para> object from file specified by argument <para>testFile</para>.
		/// </summary>
		/// <param name="testFile">Path to test result path.</param>
		/// <returns><para>TestSuites</para> object read from test result file.</returns>
		protected virtual TestSuites GetTestSuites(string testFile)
		{
			using (var reader = new StreamReader(testFile, false))
			{
				var serializer = new XmlSerializer(typeof(TestSuites));
				var testSuites = (TestSuites)serializer.Deserialize(reader);
				testSuites.FilePath = testFile;

				return testSuites;
			}
		}

		/// <summary>
		/// Extract all <para>TestCase</para> object from list of <para>TestSuite</para>.
		/// </summary>
		/// <param name="testSuiteList">Collection of <para>TestSuite</para> object.</param>
		/// <returns></returns>
		protected virtual IEnumerable<TestCase> GetAllTestCases(IEnumerable<TestSuite> testSuiteList)
		{
			var testCases = new List<TestCase>();
			foreach (var testSuite in testSuiteList)
			{
				testCases = testCases.Union(testSuite.TestCases).ToList();
			}
			return testCases;
		}

		/// <summary>
		/// Extract all <para>TestCase</para> object from list of string.
		/// </summary>
		/// <param name="testResultFiles">Collection of test result file.</param>
		/// <returns>Collection of <para>TestCase</para> object.</returns>
		protected virtual IEnumerable<TestCase> GetAllTestCases(IEnumerable<string> testResultFiles)
		{
			IEnumerable<TestSuites> testSuitesList = GetTestSuites(testResultFiles);
			IEnumerable<TestSuite> testSuite = GetTestSuite(testSuitesList);
			IEnumerable<TestCase> testCases = GetAllTestCases(testSuite);

			return testCases;
		}

		/// <summary>
		/// Set result of test into test informations.
		/// </summary>
		/// <param name="testinfo"><para>TestInformation</para> object to set the result.</param>
		/// <param name="testCases">Collection of <para>TestCase</para> object.</param>
		protected virtual void SetTestResult(TestInformation testinfo, IEnumerable<TestCase> testCases)
		{
			foreach (var testItem in testinfo.TestItems)
			{
				try
				{
					var classAndCaseName = testItem.Name.Split('.');
					string className = classAndCaseName[0];
					string name = classAndCaseName[1];
					var testCase = testCases.Where(_ =>
						_.Name.Equals(name) && _.ClassName.Equals(className))
						.OrderByDescending(_ => _.Timestamp)
						.FirstOrDefault();
					testItem.Result = testCase.Judge;
				}
				catch (NullReferenceException)
				{
					testItem.Result = string.Empty;	//No result can be found in the test case.
				}
			}
		}
	}
}
