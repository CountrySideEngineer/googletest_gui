using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace gtest_gui.View.Converter
{
	[ValueConversion(typeof(Visibility), typeof(string))]
	public class Result2VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility visible = Visibility.Hidden;
			string result = value as string;
			string resultLower = result.ToLower();
			if ((string.IsNullOrEmpty(result)) || (string.IsNullOrWhiteSpace(result)))
			{
				visible = Visibility.Hidden;
			}
			else
			{
				visible = Visibility.Visible;
			}
			return visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
