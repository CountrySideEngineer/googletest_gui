using gtest_gui.View;
using gtest_gui.ViewModel;
using System;
using System.Collections.Generic;
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
			var dstViewModel = new TestLogViewModel()
			{
				LogFilePath = LogFilePath
			};
			var dstWindow = new TestLogWindow()
			{
				DataContext = dstViewModel
			};
			dstViewModel.LoadTestLogCommandExecute();
			dstWindow.ShowDialog();
		}
	}
}
