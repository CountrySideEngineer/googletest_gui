using gtest2html;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.ViewModel
{
	public class TestHistoryViewModel : ViewModelBase
	{
		/// <summary>
		/// Test name field.
		/// </summary>
		protected string _testName;

		/// <summary>
		/// Test result field.
		/// </summary>
		protected string _testResult;

		/// <summary>
		/// Field of test cases.
		/// </summary>
		protected IEnumerable<TestCase> _testCases;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestHistoryViewModel()
		{
			this.TestName = string.Empty;
			this.TestResult = string.Empty;
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

		/// <summary>
		/// Tes name property.
		/// </summary>
		public string TestName
		{
			get
			{
				return this._testName;
			}
			set
			{
				this._testName = value;
				this.RaisePropertyChanged(nameof(TestName));
			}
		}

		/// <summary>
		/// Test result property.
		/// </summary>
		public string TestResult
		{
			get
			{
				return this._testResult;
			}
			set
			{
				this.RaisePropertyChanged(nameof(TestResult));
			}
		}
	}
}
