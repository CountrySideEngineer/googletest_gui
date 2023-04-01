using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace gtest_gui.MoveWindow
{
	public class Move2OkResult : Move2Result
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Move2OkResult() : base() { }

		/// <summary>
		/// Move to dialog to notify success.
		/// </summary>
		/// <param name="srcContext">Data context.</param>
		public override void Move(object srcContext)
		{
			try
			{
				base.ShowDialog(MessageBoxImage.None);
			}
			catch (Exception)
			{
				//Ignore an exception thrown while displaying message box.
			}
		}
	}
}
