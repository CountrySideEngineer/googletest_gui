using CountrySideEngineer.ViewModel.Base;
using gtest_gui.Command.Exception;
using gtest_gui.MoveWindow;
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

		/// <summary>
		/// Raise event to notify error.
		/// </summary>
		/// <param name="e"></param>
		public virtual void RaiseNotifyErrorEvent(EventArgs e)
		{
			NotifyError(null);
		}

		/// <summary>
		/// Raise event to notify success.
		/// </summary>
		/// <param name="e"></param>
		public virtual void RaiseNotifySuccessEvent(EventArgs e)
		{
			NotifySuccess(null);
		}

		public virtual void NotifyError(object resultData)
		{
			var mover = new Move2NgResult();
			var exception = (CommandException)resultData;

			try
			{
				mover.Title = exception.Title;
				mover.Message = exception.Summary;
			}
			catch (Exception ex)
			when ((ex is NullReferenceException) || (ex is InvalidCastException))
			{
				mover.Title = "コマンドエラー";
				mover.Message = "処理中にエラーが発生しました。";
			}
			MoveWindow(mover);
		}

		public virtual void NotifySuccess(object resultData)
		{
			IMoveWindow mover = new Move2OkResult()
			{
				Title = "成功",
				Message = "処理が完了しました。"
			};
			MoveWindow(mover);
		}

		/// <summary>
		/// Move to a dialog, window, to show a result.
		/// </summary>
		protected virtual void MoveWindow(IMoveWindow mover)
		{
			mover.Move(this);
		}
	}
}
