using gtest_gui.Model;
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
		protected List<TestCase> _testCases;

		/// <summary>
		/// Field of test item.
		/// </summary>
		protected TestItem _testItem;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestHistoryViewModel()
		{
			this._testCases = null;
			this._testItem = null;
			this.TestName = string.Empty;
			this.TestResult = string.Empty;
		}

		/// <summary>
		/// List of test case, TestCase object.
		/// </summary>
		public List<TestCase> TestCases
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
				this._testResult = value;
				this.RaisePropertyChanged(nameof(TestResult));
			}
		}

		/// <summary>
		/// Test item to show history in this view model.
		/// </summary>
		public TestItem TestItem
		{
			get
			{
				return this._testItem;
			}
			set
			{
				this._testItem = value;
				try
				{
					this.TestName = this._testItem.Name;
					this.TestResult = this._testItem.Result;
				}
				catch (NullReferenceException)
				{
					this.TestName = string.Empty;
					this.TestResult = string.Empty;
				}
			}
		}
	}
}
