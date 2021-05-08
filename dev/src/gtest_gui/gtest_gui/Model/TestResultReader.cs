using gtest2html;
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
		/// Read test data corresponding to the test execution file.
		/// </summary>
		/// <param name="testInfo">Test information file includes the test execution file.</param>
		public void ReadTest(TestInformation testInfo)
		{
			try
			{
				string testFileName = Path.GetFileNameWithoutExtension(testInfo.TestFile);
				IEnumerable<string> testResultFiles = this.GetTestResultFiles(testFileName);
				IEnumerable<TestCase> testCases = this.GetAllTestCases(testResultFiles);
				this.SetTestResult(testInfo, testCases);
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
		protected IEnumerable<string> GetTestResultFiles(string fileName)
		{
			string dirPath = @".\log";
			List<string> testResultFiles = null;
			if (Directory.Exists(dirPath))
			{
				string logFileTemplate = fileName + "_*.xml";
				string[] xmlFiles = Directory.GetFiles(@".\log", logFileTemplate);
				testResultFiles = new List<string>(xmlFiles);
			}
			else
			{
				throw new DirectoryNotFoundException();
			}
			return testResultFiles;
		}

		/// <summary>
		/// Get <para>TestSuites</para> objects from files <para>testFiles</para>.
		/// </summary>
		/// <param name="testFiles">Collection of test result file path.</param>
		/// <returns>Colelction of <para>TestSuites</para> read from files.</returns>
		protected IEnumerable<TestSuites> GetTestSuites(IEnumerable<string> testFiles)
		{
			var testSuitesList = new List<TestSuites>();
			foreach (var testFile in testFiles)
			{
				var testSuites = this.GetTestSuites(testFile);
				testSuitesList.Add(testSuites);

			}
			return testSuitesList;
		}

		/// <summary>
		/// Extract <para>TestSuite</para> objects from collection of <para>TestSuites</para>.
		/// </summary>
		/// <param name="testSuitesList">Collection of <para>TestSuites</para> object.</param>
		/// <returns>Collection of <para>TestSuite</para>.</returns>
		protected IEnumerable<TestSuite> GetTestSuite(IEnumerable<TestSuites> testSuitesList)
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
		protected TestSuites GetTestSuites(string testFile)
		{
			using (var reader = new StreamReader(testFile, false))
			{
				var serializer = new XmlSerializer(typeof(TestSuites));
				var testSuites = (TestSuites)serializer.Deserialize(reader);

				return testSuites;
			}
		}

		/// <summary>
		/// Extract all <para>TestCase</para> object from list of <para>TestSuite</para>.
		/// </summary>
		/// <param name="testSuiteList">Collection of <para>TestSuite</para> object.</param>
		/// <returns></returns>
		protected IEnumerable<TestCase> GetAllTestCases(IEnumerable<TestSuite> testSuiteList)
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
		protected IEnumerable<TestCase> GetAllTestCases(IEnumerable<string> testResultFiles)
		{
			IEnumerable<TestSuites> testSuitesList = this.GetTestSuites(testResultFiles);
			IEnumerable<TestSuite> testSuite = this.GetTestSuite(testSuitesList);
			IEnumerable<TestCase> testCases = this.GetAllTestCases(testSuite);

			return testCases;
		}

		/// <summary>
		/// Set result of test into test informations.
		/// </summary>
		/// <param name="testinfo"><para>TestInformation</para> object to set the result.</param>
		/// <param name="testCases">Collection of <para>TestCase</para> object.</param>
		protected void SetTestResult(TestInformation testinfo, IEnumerable<TestCase> testCases)
		{
			foreach (var testItem in testinfo.TestItems)
			{
				try
				{
					var classAndCaseName = testItem.Name.Split(".");
					string className = classAndCaseName[0];
					string name = classAndCaseName[1];
					var testCase = testCases.Where(_ =>
						_.Name.Equals(name) && _.ClassName.Equals(className))
						.OrderBy(_ => _.Timestamp)
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
