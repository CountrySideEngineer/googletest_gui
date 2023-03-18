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
			var dstViewModel = new TestHistoryViewModel
			{
				TestFilePath = srcViewModel.TestInfo.TestFile,
				TestItem = testItem
			};
			var historyWindow = new TestHistoryWindow()
			{
				DataContext = dstViewModel
			};
			dstViewModel.LoadTestHistoryCommandExecute();
			historyWindow.ShowDialog();
		}
	}
}
