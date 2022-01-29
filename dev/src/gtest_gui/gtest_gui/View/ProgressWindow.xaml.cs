using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gtest_gui.View
{
	/// <summary>
	/// ProgressWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class ProgressWindow : Window
	{
		[DllImport("user32.dll")]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		const int GWL_STYLE = -16;
		const int WS_SYS_MENU = 0x00080000;

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			IntPtr handle = new WindowInteropHelper(this).Handle;
			int style = GetWindowLong(handle, GWL_STYLE);
			style = style & (~WS_SYS_MENU);
			SetWindowLong(handle, GWL_STYLE, style);
		}



		public ProgressWindow()
		{
			InitializeComponent();
		}
	}
}
