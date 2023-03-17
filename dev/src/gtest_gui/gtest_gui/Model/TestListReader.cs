using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace gtest_gui.Model
{
    public class TestListReader
    {
        /// <summary>
        /// Path to test exeuction file.
        /// </summary>
        public string TestFilePath { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestListReader()
		{
            TestFilePath = string.Empty;
		}

        /// <summary>
        /// Constructor with argument.
        /// </summary>
        /// <param name="testFilePath">Path to file to get the list of test to execute.</param>
        public TestListReader(string testFilePath)
		{
            TestFilePath = testFilePath;
		}

        /// <summary>
        /// Read test items.
        /// </summary>
        /// <returns>Collection of test case as a IEnumerable object.</returns>
        public virtual IEnumerable<TestItem> Read()
		{
            IEnumerable<TestItem> items = Read(TestFilePath, OutputToList);
            return items;
		}

        /// <summary>
        /// Read test cases as a list.
        /// </summary>
        /// <param name="path">Path to stream, file, to read file list from.</param>
        /// <param name="postTest">Function to run after reading process.</param>
        /// <returns>Collection of TestItem object.</returns>
        public virtual IEnumerable<TestItem> Read(
            string path, 
            Func<StreamReader, IEnumerable<TestItem>> postTest)
		{
            ProcessStartInfo startInfo = GetProcessStartInfo();
            startInfo.FileName = path;
            startInfo.Arguments = "--gtest_list_tests";

            IEnumerable<TestItem> items = RunProcess(startInfo, postTest);

            return items;
		}

        /// <summary>
        /// Run process to read test list.
        /// </summary>
        /// <param name="procInfo">Process information to run.</param>
        /// <param name="postProcess">Function to run after the process finished.</param>
        /// <returns>Collection of TestItem object.</returns>
        protected virtual IEnumerable<TestItem> RunProcess(ProcessStartInfo procInfo,
            Func<StreamReader, IEnumerable<TestItem>> postProcess)
		{
            using (var process = new Process())
			{
                process.StartInfo = procInfo;
                process.Start();
                process.WaitForExit();

                IEnumerable<TestItem> testItems = postProcess?.Invoke(process.StandardOutput);

                return testItems;
			}
		}

        /// <summary>
        /// Get ProcessStartInfo object to be used in process to read test list..
        /// </summary>
        /// <returns>ProcessStartInfo object.</returns>
        protected virtual ProcessStartInfo GetProcessStartInfo()
		{
            var procStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            return procStartInfo;
		}

        /// <summary>
        /// Output test list read from stream to <para>testInformation.</para>
        /// </summary>
        /// <param name="testOutputStream">Stream to read test list from.</param>
        /// <param name="testInformation"><para>TestInformation</para> object to set test information read from stream.</param>
        protected virtual IEnumerable<TestItem> OutputToList(StreamReader testOutputStream)
		{
            string outputData = testOutputStream.ReadToEnd();
            IEnumerable<TestItem> testItems = OutputToTestItem(outputData);
            return testItems;
		}

        /// <summary>
        /// Convert output of google test file.
        /// </summary>
        /// <param name="output">Standard output data.</param>
        /// <returns>List of test items read from <para>output</para>.</returns>
        protected virtual IEnumerable<TestItem> OutputToTestItem(string output)
		{
            var outputInArray = output.Replace("\r\n", "\n").Split(new[] { '\n', '\r'});
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
