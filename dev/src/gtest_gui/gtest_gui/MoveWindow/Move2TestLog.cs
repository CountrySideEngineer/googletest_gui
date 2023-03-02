using gtest_gui.View;
using gtest_gui.ViewModel;
using gtest2html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gtest_gui.MoveWindow
{
	public class Move2TestLog : IMoveWindow
	{
		/// <summary>
		/// Path to file to show in the window.
		/// </summary>
		public string LogFilePath { get; set; }

		/// <summary>
		/// Move to test log window.
		/// </summary>
		/// <param name="srcContext">Data to show in the window.</param>
		public void Move(object srcContext)
		{
			var viewModel = (TestHistoryViewModel)srcContext;
			int selectedIndex = viewModel.SelectedIndex;
			IEnumerable<TestCase> testCases = viewModel.TestCases;
			TestCase testCase = testCases.ElementAt(selectedIndex);
			var dstViewModel = new TestLogViewModel(testCase)
			{
				TestInformation = viewModel.TestInformation
			};
			var dstWindow = new TestLogWindow()
			{
				DataContext = dstViewModel
			};
			dstViewModel.GetTestLogFilePathCommandExecute();
			dstViewModel.LoadTestLogCommandExecute();
			dstWindow.ShowDialog();
		}
	}
}
