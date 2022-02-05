using gtest_gui.Command;
using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountrySideEngineer.ViewModel.Base;

namespace gtest_gui.ViewModel
{
	public class TestHistoryViewModel : ViewModelBase
	{
		/// <summary>
		/// Field of test cases.
		/// </summary>
		protected List<TestCase> _testCases;

		/// <summary>
		/// Test name.
		/// </summary>
		protected TestInformation _testName;

		/// <summary>
		/// Test information.
		/// </summary>
		protected TestInformation _testInformation;

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

		public string TestName
		{
			get
			{
				try
				{
					return this.TestInformation.TestItems.ElementAt(0).Name;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}

		public TestInformation TestInformation
		{
			get
			{
				return this._testInformation;
			}
			set
			{
				this._testInformation = value;
				this.RaisePropertyChanged(nameof(TestInformation));
				this.RaisePropertyChanged(nameof(TestName));
			}
		}

		public void LoadTestHistoryCommandExecute()
		{
			var commandArg = new TestCommandArgument(this.TestInformation);
			var command = new LoadTestHistoryCommand();
			IEnumerable<TestCase> testCases = (IEnumerable<TestCase>)command.ExecuteCommand(commandArg);
			List<TestCase> testCaseList = testCases.ToList();
			this.TestCases = testCaseList;
		}
	}
}
