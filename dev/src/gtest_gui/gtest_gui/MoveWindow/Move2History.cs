using gtest_gui.View;
using gtest_gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using gtest_gui.Model;
using gtest2html;

namespace gtest_gui.MoveWindow
{
	public class Move2History : IMoveWindow
	{
		/// <summary>
		/// Move to test history window.
		/// </summary>
		/// <param name="srcContext">Data to show in the window.</param>
		public void Move(object srcContext)
		{
			var srcViewModel = (GTestGuiViewModel)srcContext;
			int selectedTestIndex = srcViewModel.SelectedTestIndex;
			TestItem testItem = srcViewModel.TestInfo.TestItems.ElementAt(selectedTestIndex);
			string testFilePath = srcViewModel.TestInfo.TestFile;
			TestInformation testInfo = new TestInformation
			{
				TestFile = testFilePath,
				TestItems = new List<TestItem>
				{
					testItem
				}
			};

			var dstViewModel = new TestHistoryViewModel
			{
				TestInformation = testInfo
			};
			var historyWindow = new TestHistoryWindow()
			{
				DataContext = dstViewModel
			};
			dstViewModel.LoadTestHistoryCommandExecute();
			historyWindow.ShowDialog();
		}

		protected IEnumerable<TestCase> ExtractTestHistory(string testFilePath, TestItem testItem)
		{
			var testInfo = new TestInformation()
			{
				TestFile = testFilePath,
				TestItems = new List<TestItem>
				{
					testItem
				}
			};
			var reader = new TestHistoryReader();
			IEnumerable<TestCase> testCases = reader.ReadTest(testInfo);
			return testCases;
		}
	}
}
