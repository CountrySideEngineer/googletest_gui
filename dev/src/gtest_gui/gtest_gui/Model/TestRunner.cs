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
                this.Run(path, testItem, this.OutputLog);
            }
        }

        /// <summary>
        /// Run a test 
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="testItem">Test information.</param>
        /// <returns>Test running process.</returns>
        protected virtual Process Run(string path, TestItem testItem, Action<StreamReader, TestItem> postTest)
		{
            string filterOption = this.GetFilterOption(path, testItem);
            string outputOption = this.GetXmlOutputOption(path, testItem);
            Process process = null;
            var procStartInfo = new ProcessStartInfo
            {
                FileName = path,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"{outputOption} {filterOption}"
            };
            using (process = new Process())
			{
                process.StartInfo = procStartInfo;
                process.Start();
                process.WaitForExit();

                postTest?.Invoke(process.StandardOutput, testItem);
            }
            return process;
		}

        /// <summary>
        /// Output log into file.
        /// </summary>
        /// <param name="testOutputStream">Stream to read log from.</param>
        /// <param name="testItem">Test item to run.</param>
        protected virtual void OutputLog(StreamReader testOutputStream, TestItem testItem)
		{
            string outputData = testOutputStream.ReadToEnd();
            string logFilePath = this.OutputDirFile.LogFilePath(testItem.Name);
            using (var writer = new StreamWriter(logFilePath))
			{
                writer.Write(outputData);
			}
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
