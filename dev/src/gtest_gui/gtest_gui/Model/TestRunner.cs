using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Delegate to raise event that a test data receive finished event.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event argument</param>
        public delegate void TestDataFinishedEvent(object sender, TestDataFinishedEventArgs e);

        /// <summary>
        /// Event handler to handle a test finished.
        /// </summary>
        public event TestDataFinishedEvent TestDataFinisedEventHandler;

        /// <summary>
        /// Delegate to raise event that test output received event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event argument.</param>
        public delegate void TestDataReceivedEvent(object sender, DataReceivedEventArgs e);

        /// <summary>
        /// Event handler to handle data received.
        /// </summary>
        public event TestDataReceivedEvent TestDataReceivedEventHandler;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TestRunner()
		{
            Target = string.Empty;
            OutputDirFile = null;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        public TestRunner(string target)
        {
            Target = target;
            OutputDirFile = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Path to test file.</param>
        /// <param name="outputDirFile">OutputDirAndFile object handle test log and report.</param>
        public TestRunner(string target, OutputDirAndFile outputDirFile)
        {
            Target = target;
            OutputDirFile = outputDirFile;
        }

        /// <summary>
        /// Run test.
        /// </summary>
        public void Run(TestInformation information)
		{
            Run(Target, information);
		}

        /// <summary>
        /// Run a test item.
        /// </summary>
        /// <param name="testItem">Test data to run.</param>
        public virtual void Run(TestItem testItem)
		{
            Run(Target, testItem);
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
                RunTestProc(path, testItem);
            }
        }

        /// <summary>
        /// Run a test 
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="testItem">Test information.</param>
        /// <returns>Test running process.</returns>
        protected virtual Process Run(string path, TestItem testItem)
		{
            ProcessStartInfo procStartInfo = GetProcessStartInfo(path, testItem);
            Process process = null;

            using (process = new Process())
			{
                process.StartInfo = procStartInfo;
                process.ErrorDataReceived += OnErrorDataReceivedEvent;
                process.OutputDataReceived += OnOutputDataReceivedEvent;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                process.CancelOutputRead();
                process.OutputDataReceived -= OnOutputDataReceivedEvent;
                process.ErrorDataReceived -= OnErrorDataReceivedEvent;
            }
            return process;
		}

        /// <summary>
        /// Run a test case with pre- and post procedure.
        /// Pre-procedure is to setup directories to output log and report.
        /// Post-procedure is to notify a test finished by event.
        /// </summary>
        /// <param name="path">Path to file to execute.</param>
        /// <param name="testItem">Test item information.</param>
        public virtual void RunTestProc(string path, TestItem testItem)
		{
            //pre-procedure.
            OutputDirFile.SetUpTestOutputDirectories(testItem.Name);

            Run(path, testItem);

            //post procedure.
            var eventArg = new TestDataFinishedEventArgs()
            {
                TestItem = testItem
            };
            TestDataFinisedEventHandler?.Invoke(this, eventArg);
		}

        /// <summary>
        /// Data received from execution process output.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">Event argument.</param>
        protected virtual void OnOutputDataReceivedEvent(object sender, DataReceivedEventArgs e)
		{
            if (null != e.Data)
			{
                TestDataReceivedEventHandler?.Invoke(this, e);
			}
		}

        /// <summary>
        /// Ouptut standard error content to file.
        /// </summary>
        /// <param name="sender">Evnet sender.</param>
        /// <param name="e">Event arguent.</param>
        protected virtual void OnErrorDataReceivedEvent(object sender, DataReceivedEventArgs e)
        {
            if (null != e.Data)
            {
                TestDataReceivedEventHandler?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Get ProcessStartInformation object to run a test.
        /// </summary>
        /// <param name="path">Path to test execution file.</param>
        /// <param name="item">Test item data.</param>
        /// <returns>ProcessStartInformation object to run test with default parameter.</returns>
        protected virtual ProcessStartInfo GetProcessStartInfo(string path, TestItem item)
		{
            ProcessStartInfo procInfo = new ProcessStartInfo()
            {
                FileName = path,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            return procInfo;
        }
    }
}
