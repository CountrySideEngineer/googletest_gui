using CountrySideEngineer.ViewModel.Base;
using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.ViewModel
{
	public class TestLogViewModel : ViewModelBase
	{
		/// <summary>
		/// Path to log file to display.
		/// </summary>
		protected string _logFilePath;

		/// <summary>
		/// Field of window title.
		/// </summary>
		protected string _windowTitle;

		/// <summary>
		/// Logs loaded from file specified by _logFilePath;
		/// </summary>
		protected string _logContent;

		/// <summary>
		/// Test information data.
		/// </summary>
		protected TestInformation _testInfo;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestLogViewModel()
		{

		}

		/// <summary>
		/// Window title property.
		/// </summary>
		public string WindowTitle
		{
			get
			{
				return _logFilePath;
			}
			set
			{
				_logFilePath = value;
				RaisePropertyChanged(nameof(WindowTitle));
			}
		}

		/// <summary>
		/// Log content property.
		/// </summary>
		public string LogContent
		{
			get
			{
				return _logContent;
			}
			set
			{
				_logContent = value;
				RaisePropertyChanged(nameof(LogContent));
			}
		}
	}
}
