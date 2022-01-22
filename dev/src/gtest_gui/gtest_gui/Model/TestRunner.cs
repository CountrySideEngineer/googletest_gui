using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.Model
{
    public class TestRunner
    {
        /// <summary>
        /// Path to test exeuction file.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Test log and file information.
        /// </summary>
        public OutputDirAndFile OutputDirFile { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestRunner()
		{
            this.Target = string.Empty;
            this.OutputDirFile = null;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        public TestRunner(string target)
        {
            this.Target = target;
            this.OutputDirFile = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        /// <param name="outputDirFile">OutputDirAndFile object handle test log and report.</param>
        public TestRunner(string target, OutputDirAndFile outputDirFile)
        {
            this.Target = target;
            this.OutputDirFile = outputDirFile;
        }

        /// <summary>
        /// Run test.
        /// </summary>
        public void Run(TestInformation information)
		{
            this.Run(this.Target, information);
		}

        /// <summary>
        /// Run test
        /// </summary>
        /// <param name="path">Path to file to run test.</param>
        public virtual void Run(string path, TestInformation information)
		{
            var targetTestItems = information.TestItems.Where(_ => _.IsSelected);
            foreach (var testItem in targetTestItems)
            {
                this.OutputDirFile.SetUpTestOutputDirecries(testItem.Name);
                this.RunTest(path, testItem);
            }
        }

        /// <summary>
        /// Run a test.
        /// </summary>
        /// <param name="path">Path to file to run test.</param>
        /// <param name="testItem">Test parameter.</param>
        protected virtual void RunTest(string path, TestItem testItem)
		{
            Process process = this.Start(path, testItem);
            process.WaitForExit();

            //Get and output log 
            string outputData = process.StandardOutput.ReadToEnd();
            string logFilePath = this.OutputDirFile.LogFilePath(testItem.Name);
            using (var writer = new StreamWriter(logFilePath))
			{
                writer.Write(outputData);
            }
        }

        /// <summary>
        /// Run a test 
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="testItem">Test information.</param>
        /// <returns>Test running process.</returns>
        protected virtual Process Start(string path, TestItem testItem)
		{
            string filterOption = this.GetFilterOption(path, testItem);
            string outputOption = this.GetXmlOutputOption(path, testItem);
            var app = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = $"{outputOption} {filterOption}"
            };
            Process process = this.Start(app);
            return process;
		}

        /// <summary>
        /// Run test execution file.
        /// </summary>
        /// <param name="processInfo">Proces object to run test.</param>
        /// <returns>Process object the test run.</returns>
        protected virtual Process Start(ProcessStartInfo processInfo)
		{
            Process proc = Process.Start(processInfo);

            return proc;
		}

        /// <summary>
        /// Get test information
        /// </summary>
        /// <returns>Test information object.</returns>
        public virtual TestInformation GetTestList()
		{
            TestInformation testInformation = this.GetTestList(this.Target);
            return testInformation;
		}

        /// <summary>
        /// Get test information.
        /// </summary>
        /// <param name="path">Path to test file.</param>
        /// <returns>Test information object</returns>
        public virtual TestInformation GetTestList(string path)
		{
            //Setup test configuration.
			var app = new ProcessStartInfo
			{
				FileName = path,
				Arguments = "--gtest_list_tests",
				UseShellExecute = false,
				RedirectStandardOutput = true
			};

			Process proc = this.Start(app);
            string stdOutput = proc.StandardOutput.ReadToEnd();
            IEnumerable<TestItem> testItems = this.OutputToTestItem(stdOutput);
            var testInfo = new TestInformation
            {
                TestFile = path,
                TestItems = testItems
            };

            return testInfo;
        }

        /// <summary>
        /// Convert output of google test file.
        /// </summary>
        /// <param name="output">Standard output data.</param>
        /// <returns>List of test items read from <para>output</para>.</returns>
        protected IEnumerable<TestItem> OutputToTestItem(string output)
		{
            var outputInArray = output.Split("\r\n");
            /*
             * The head item in array is expalanation of test, for example test file.
             * Skip the data because it is not information about test case.
             */
            outputInArray = outputInArray.Where((source, index) => 0 < index).ToArray();

            var testSuiteName = string.Empty;
            var testItems = new List<TestItem>();
            foreach (var item in outputInArray)
			{
                if (item.EndsWith("."))
				{
                    testSuiteName = item;
				}
				else
				{
                    var testName = item.Trim();
                    if (!(string.IsNullOrEmpty(testName)))
					{
                        var testItem = new TestItem
                        {
                            Name = testSuiteName + testName,
                            IsSelected = false
                        };
                        testItems.Add(testItem);
                    }
                }
			}
            return testItems;
		}

        /// <summary>
        /// Create test filter option of google test framework.
        /// </summary>
        /// <param name="fileName">Test file path.</param>
        /// <param name="testItem">Test item information.</param>
        /// <returns>Test filter string.</returns>
        protected string GetFilterOption(string fileName, TestItem testItem)
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
        protected string GetXmlOutputOption(string fileName, TestItem testItem)
		{
            string xmlFilePath = this.OutputDirFile.TestReportFilePath(testItem.Name);
            string xmlOption = $"--gtest_output=xml:{xmlFilePath}";
            return xmlOption;
		}
    }
}
