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
            this.TestFilePath = string.Empty;
		}

        /// <summary>
        /// Constructor with argument.
        /// </summary>
        /// <param name="testFilePath">Path to file to get the list of test to execute.</param>
        public TestListReader(string testFilePath)
		{
            this.TestFilePath = testFilePath;
		}

        /// <summary>
        /// Read test list.
        /// </summary>
        /// <param name="testFilePath">Path to file to read.</param>
        public virtual TestInformation Run()
		{
            TestInformation testInformation = this.Run(this.TestFilePath, this.OutputToList);
            return testInformation;
		}

        /// <summary>
        /// Read test list.
        /// </summary>
        /// <param name="testFilePath">Path to file to read.</param>
        /// <param name="testInformation"><para>TestInformation</para> object to set test list.</param>
        public virtual TestInformation Run(string testFilePath)
		{
            TestInformation testInformation = this.Run(testFilePath, this.OutputToList);
            return testInformation;
		}

        /// <summary>
        /// Read test list.
        /// </summary>
        /// <param name="testFilePath">Path to file to read.</param>
        /// <param name="testInformation"><para>TestInformation</para> object to set test list.</param>
        /// <param name="postTest">Action to run after read.</param>
        public virtual TestInformation Run(string testFilePath,
            Func<StreamReader, IEnumerable<TestItem>> postTest)
		{
            var procStartInfo = new ProcessStartInfo
            {
                FileName = testFilePath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = "--gtest_list_tests",
            };
            using (var process = new Process())
			{
                process.StartInfo = procStartInfo;
                process.Start();
                process.WaitForExit();

                IEnumerable<TestItem> testItems = postTest?.Invoke(process.StandardOutput);
                var testInformation = new TestInformation();
                testInformation.TestFile = testFilePath;
                testInformation.TestItems = testItems;
                return testInformation;
			}
		}

        /// <summary>
        /// Output test list read from stream to <para>testInformation.</para>
        /// </summary>
        /// <param name="testOutputStream">Stream to read test list from.</param>
        /// <param name="testInformation"><para>TestInformation</para> object to set test information read from stream.</param>
        protected virtual IEnumerable<TestItem> OutputToList(StreamReader testOutputStream)
		{
            string outputData = testOutputStream.ReadToEnd();
            IEnumerable<TestItem> testItems = this.OutputToTestItem(outputData);
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
