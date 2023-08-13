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

		private void HighLightText()
		{
			HighLightTextOK();
			HighLightTextNG();
		}

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
