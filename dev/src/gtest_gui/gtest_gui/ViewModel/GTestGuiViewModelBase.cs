using CountrySideEngineer.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.ViewModel
{
	public class GTestGuiViewModelBase : ViewModelBase
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public GTestGuiViewModelBase(): base() { }

		public virtual void RaiseNotifyErrorEvent(EventArgs e)
		{

		}

		public virtual void RaiseNotifySuccessEvent(EventArgs e)
		{

		}

		protected virtual void ShowErrorDialog()
		{

		}

		/// <summary>
		/// Move to a dialog, window, to show a result.
		/// </summary>
		protected virtual void Move2ResultDialog() { }
	}
}
