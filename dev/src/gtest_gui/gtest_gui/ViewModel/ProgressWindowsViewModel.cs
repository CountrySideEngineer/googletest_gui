using gtest_gui.Command.Argument;
using gtest_gui.Model;
using gtest_gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.MoveWindow
{
	public class ProgressWindowsViewModel : ViewModelBase
	{
		/// <summary>
		/// Field of title.
		/// </summary>
		protected string _title;

		/// <summary>
		/// Field of process nmae.
		/// </summary>
		protected string _processName;

		/// <summary>
		/// Field of progress, between 0 and 100.
		/// </summary>
		protected int _progress;

		/// <summary>
		/// Numerator of progress.
		/// </summary>
		protected int _numerator;

		/// <summary>
		/// Denominator of prgress.
		/// </summary>
		protected int _denominator;

		/// <summary>
		/// Delegate to handle "CloseWindow" event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void CloseWindowsEventHandler(object sender, EventArgs e);
		public CloseWindowsEventHandler CloseWindowEvent;

		/// <summary>
		/// Delegate to handle "StartPogress" event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void StartProgressEventHandler(object sender, EventArgs e);
		public StartProgressEventHandler StartProgressEvent;

		/// <summary>
		/// Property of title.
		/// </summary>
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				RaisePropertyChanged(nameof(Title));
			}
		}

		/// <summary>
		/// Property of process name.
		/// </summary>
		public string ProcessName
		{
			get => _processName;
			set
			{
				_processName = value;
				RaisePropertyChanged(nameof(ProcessName));
			}
		}

		/// <summary>
		/// Property of prgresss.
		/// </summary>
		public int ProgressRate
		{
			get => _progress;
			set
			{
				_progress = value;
				RaisePropertyChanged(nameof(ProgressRate));
			}
		}

		/// <summary>
		/// Property of numerator.
		/// </summary>
		public int Numerator
		{
			get => _numerator;
			set
			{
				_numerator = value;
				RaisePropertyChanged(nameof(Numerator));
			}
		}

		/// <summary>
		/// Test Progress interface.
		/// </summary>
		public IProgress<ProgressInfo> Progress { get; set; }

		/// <summary>
		/// Task to run in asynchronously.
		/// </summary>
		public IAsyncTask<ProgressInfo> AsyncTask { get; set; }

		/// <summary>
		/// Property of denominator.
		/// </summary>
		public int Denominator
		{
			get => _denominator;
			set
			{
				_denominator = value;
				RaisePropertyChanged(nameof(Denominator));
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ProgressWindowsViewModel()
		{
			Title = string.Empty;
			ProcessName = string.Empty;
			ProgressRate = 0;
			Numerator = 0;
			Denominator = 0;
			Progress = null;
		}

		/// <summary>
		/// Event handler notifying start the task.
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Event argument.</param>
		public void OnProgressStart(object sender, EventArgs e)
		{
			AsyncTask.RunTask(Progress);
		}

		/// <summary>
		/// Event handler of progress changed.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="arg">Event argument.</param>
		/// <remarks>If the "progress" value is greater than 100, this method publishes window close event.</remarks>
		public void OnProgressChanged(object sender, ProgressChangedCommandArgument arg)
		{
			ProgressInfo progressInfo = arg.ProgressInfo;
			Title = progressInfo.Title;
			ProcessName = progressInfo.ProcessName;
			ProgressRate = progressInfo.Progress;
			Numerator = progressInfo.Numerator;
			Denominator = progressInfo.Denominator;

			if (100 <= ProgressRate)
			{
				CloseWindowEvent?.Invoke(this, null);
			}
		}
	}
}
