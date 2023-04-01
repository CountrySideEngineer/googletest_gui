using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace gtest_gui.MoveWindow
{
	public class Move2NgResult : Move2Result
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Move2NgResult() : base() { }

		/// <summary>
		///	Move to a dialog to notify failure, NG.
		/// </summary>
		/// <param name="srcContext">Data context.</param>
		public override void Move(object srcContext)
		{
			try
			{
				base.ShowDialog(MessageBoxImage.Error);
			}
			catch (Exception)
			{
				//Ignore an exception thrown while displaying message box.
			}
		}
	}
}
