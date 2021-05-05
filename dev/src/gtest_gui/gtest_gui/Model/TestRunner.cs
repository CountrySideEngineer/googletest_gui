using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Path to test report file.
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestRunner()
		{
            this.Target = string.Empty;
            this.Report = string.Empty;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        public TestRunner(string target)
        {
            this.Target = target;
            this.Report = string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        /// <param name="report">Path to report file to output.</param>
        public TestRunner(string target, string report)
        {
            this.Target = target;
            this.Report = report;
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
        /// <param name="path">Path to fine to run.</param>
        public virtual void Run(string path, TestInformation information)
		{
            var targetTestItems = information.TestItems.Where(_ => _.IsSelected);
            var testFilterOption = string.Empty;
            bool isTop = true;
            foreach (var item in targetTestItems)
			{
                if (isTop)
				{
                    testFilterOption = "--gtest_filter=";   //Option prefix to filter test case.
				}
				else
				{
                    testFilterOption += ":";
				}
                testFilterOption += item.Name;
                isTop = false;
			}
			var app = new ProcessStartInfo
			{
				FileName = path,
				UseShellExecute = false,
				Arguments = testFilterOption,
			};
            Process proc = this.Run(app);
            proc.WaitForExit();
		}

        /// <summary>
        /// Run test execution file.
        /// </summary>
        /// <param name="processInfo">Proces object to run test.</param>
        /// <returns>Process object the test run.</returns>
        protected virtual Process Run(ProcessStartInfo processInfo)
		{
            Process proc = Process.Start(processInfo);

            return proc;
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

			Process proc = this.Run(app);
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
    }
}
