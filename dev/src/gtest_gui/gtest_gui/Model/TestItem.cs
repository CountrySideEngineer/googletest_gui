using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using gtest2html;
using CountrySideEngineer.ViewModel.Base;

namespace gtest_gui.Model
{
	public class TestItem : ViewModelBase
	{
		/// <summary>
		/// Test name
		/// </summary>
		public string Name { get; set; }

		bool _isSelected = false;
		/// <summary>
		/// Get and set the value indicates whether the test item is selected to run or not.
		/// </summary>
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				RaisePropertyChanged(nameof(IsSelected));
			}
		}

		/// <summary>
		/// Result of test.
		/// </summary>
		public string Result { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestItem()
		{
			this.Name = string.Empty;
			this.IsSelected = false;
			this.Result = string.Empty;
		}

		/// <summary>
		/// Determine whether the test datas are equal.
		/// </summary>
		/// <param name="obj">Instance to compare</param>
		/// <returns>Returns true if the specified object equal to the current object.
		/// Otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			bool isEqual = false;
			try
			{
				var src = (TestItem)obj;
				if (this.Name.Equals(src.Name))
				{
					isEqual = true;
				}
				else
				{
					isEqual = false;
				}
			}
			catch (InvalidCastException)
			{
				isEqual = false;
			}
			return isEqual;
		}
	}
}
