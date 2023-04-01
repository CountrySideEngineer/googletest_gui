using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace gtest_gui.MoveWindow
{
	public class Move2Result : IMoveWindow
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Move2Result() : base()
		{
			Title = string.Empty;
			Message = string.Empty;
		}

		/// <summary>
		/// Result window title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Move to a window to notify a result.
		/// </summary>
		/// <param name="srcContext"></param>
		public virtual void Move(object srcContext)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Show a message box.
		/// </summary>
		/// <param name="image">Image data to show in Message box.</param>
		protected virtual void ShowDialog(MessageBoxImage image)
		{
			MessageBox.Show(Message, Title, MessageBoxButton.OK, image);
		}
	}
}
