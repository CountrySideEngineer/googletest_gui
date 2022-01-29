using gtest_gui.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.MoveWindow
{
	public class Move2Progress : IMoveWindow
	{
		public virtual void Move(object srcContext)
		{
			var viewModel = new ProgressWindowsViewModel();
			var progressWindow = new ProgressWindow()
			{
				DataContext = viewModel
			};
			progressWindow.ShowDialog();
		}
	}
}
