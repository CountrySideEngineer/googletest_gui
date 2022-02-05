using CountrySideEngineer.ProgressWindow.Model;
using CountrySideEngineer.ProgressWindow.Model.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace gtest_gui.Model
{
	public class TestRunnerAsync : IAsyncTask<ProgressInfo>
	{
		/// <summary>
		/// Test runner
		/// </summary>
		public TestRunner TestRunner { get; set; }

		/// <summary>
		/// Test information for TestRunner object.
		/// </summary>
		public TestInformation TestInfo { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestRunnerAsync()
		{
			TestRunner = null;
			TestInfo = null;
		}

		/// <summary>
		/// Implement of interface.
		/// Start test running.
		/// </summary>
		public virtual void RunTask(IProgress<ProgressInfo> progress)
		{
			Task task = Run(TestRunner, TestInfo, progress);
		}

		/// <summary>
		/// Run task.
		/// </summary>
		/// <param name="runner">Test runner.</param>
		/// <param name="testInformation">Test information.</param>
		/// <param name="progress">Interface to notify progress.</param>
		/// <returns></returns>
		protected virtual async Task Run(TestRunner runner, TestInformation testInformation, IProgress<ProgressInfo> progress)
		{
			Task task = RunTask(runner, testInformation, progress);
			await task;
		}

		protected virtual Task RunTask(TestRunner runner, TestInformation testInformation, IProgress<ProgressInfo> progress)
		{
			Task task = Task.Run(() =>
			{
				var targetTestItems = testInformation.TestItems.Where(_ => _.IsSelected);
				int testCount = targetTestItems.Count();
				var baseProgInfo = new ProgressInfo()
				{
					Title = testInformation.TestFile,
					Denominator = testCount
				};
				if (!(0 < testCount))
				{
					var endProgInfo = new ProgressInfo(baseProgInfo);
					endProgInfo.Progress = 100;
					progress.Report(endProgInfo);
					return;
				}
				int testIndex = 0;
				foreach (var testItem in targetTestItems)
				{
					var preProgInfo = new ProgressInfo(baseProgInfo);
					preProgInfo.ProcessName = testItem.Name;
					preProgInfo.Progress = (testIndex * 100) / testCount;
					preProgInfo.Numerator = testIndex;
					progress.Report(preProgInfo);

					runner.RunTestProc(TestInfo.TestFile, testItem);

					testIndex++;
					var postProgInfo = new ProgressInfo(preProgInfo);
					postProgInfo.Progress = (testIndex * 100) / testCount;
					postProgInfo.Numerator = testIndex;
					progress.Report(postProgInfo);
				}
			});
			return task;
		}
	}
}
