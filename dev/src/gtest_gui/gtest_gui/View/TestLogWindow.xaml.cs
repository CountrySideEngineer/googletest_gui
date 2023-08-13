using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gtest_gui.View
{
	/// <summary>
	/// TestLogWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class TestLogWindow : Window
	{
		public TestLogWindow()
		{
			InitializeComponent();
		}

		private void LogTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			HighLightText();
		}

		/// <summary>
		/// Highlight OK and NG words text in log window.
		/// </summary>
		private void HighLightText()
		{
			HighLightTextOK();
			HighLightTextNG();
		}

		/// <summary>
		/// Highlight words about test result OK!
		/// </summary>
		private void HighLightTextOK()
		{
			var hilightTextData = new List<string>
			{
				"[==========]",
				"[----------]",
				"[ RUN      ]",
				"[       OK ]",
				"[  PASSED  ]",
			};
			foreach (var textData in hilightTextData)
			{
				HighLightText(textData, Brushes.SpringGreen);
			}
		}

		/// <summary>
		/// Highlight word about test result NG.
		/// </summary>
		private void HighLightTextNG()
		{
			var hilightTextData = new List<string>
			{
				"[  FAILED  ]",
			};
			foreach (var textData in hilightTextData)
			{
				HighLightText(textData, Brushes.Red);
			}
		}

		/// <summary>
		/// Highlight words match with keyword match with argument keyword by argument color.
		/// </summary>
		/// <param name="keyword">A word to change format.</param>
		/// <param name="color">A color to set to the keyword.</param>
		private void HighLightText(string keyword, Brush color)
		{
			try
			{
				int leftLen = 0;
				TextPointer start = LogTextBox.Document.ContentStart;
				do
				{
					TextPointer stop = start.GetPositionAtOffset(keyword.Length);
					var wrapRange = new TextRange(start, stop);
					var wrapText = wrapRange.Text;
					if (wrapText.Equals(keyword))
					{
						wrapRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
						start = stop;
					}
					else
					{
						start = start.GetPositionAtOffset(1);
					}
					leftLen = start.GetOffsetToPosition(LogTextBox.Document.ContentEnd);
				} while ((0 < leftLen) && (keyword.Length < leftLen));
			}
			catch (ArgumentException) { }
		}
	}
}
