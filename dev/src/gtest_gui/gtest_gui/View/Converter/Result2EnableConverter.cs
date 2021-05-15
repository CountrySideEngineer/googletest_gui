using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace gtest_gui.View.Converter
{
	[ValueConversion(typeof(bool), typeof(string))]
	public class Result2EnableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = value as string;
			string resultLower = result.ToLower();
			bool isEnabled = false;
			if ((string.IsNullOrEmpty(result)) || (string.IsNullOrWhiteSpace(result)))
			{
				isEnabled = false;
			}
			else
			{
				isEnabled = true;
			}
			return isEnabled;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
