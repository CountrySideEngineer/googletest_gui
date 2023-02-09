using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace gtest_gui.View.Converter
{
	[ValueConversion(typeof(DateTime), typeof(string))]
	public class DatetimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime dateTime = (DateTime)value;
			string converted = dateTime.ToString("yyyy/MM/dd HH:mm.ss");

			return converted;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
