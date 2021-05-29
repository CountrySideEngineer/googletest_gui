using gtest_gui.View;
using gtest_gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using gtest_gui.Model;

namespace gtest_gui.MoveWindow
{
	public class Move2History : IMoveWindow
	{
		public void Move(object srcContext)
		{
			var srcViewModel = (GTestGuiViewModel)srcContext;
			int selectedTestIndex = srcViewModel.SelectedTestIndex;
			var testItems = srcViewModel.TestInfo.TestItems;
			TestItem  testItem = testItems.ElementAt(selectedTestIndex);
			var dstViewModel = new TestHistoryViewModel
			{
				TestItem = testItem
			};
			var historyWindow = new TestHistoryWindow()
			{
				DataContext = dstViewModel
			};

			historyWindow.ShowDialog();
		}
	}
}
