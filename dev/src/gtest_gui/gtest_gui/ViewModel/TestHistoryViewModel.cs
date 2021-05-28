using gtest2html;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.ViewModel
{
	public class TestHistoryViewModel : ViewModelBase
	{
		/// <summary>
		/// Field of test cases.
		/// </summary>
		protected IEnumerable<TestCase> _testCases;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestHistoryViewModel()
		{
			this._testCases = null;
		}

		/// <summary>
		/// List of test case, TestCase object.
		/// </summary>
		public IEnumerable<TestCase> TestCases
		{
			get
			{
				return this._testCases;
			}
			set
			{
				this._testCases = value;
				this.RaisePropertyChanged(nameof(this.TestCases));
			}
		}
	}
}
