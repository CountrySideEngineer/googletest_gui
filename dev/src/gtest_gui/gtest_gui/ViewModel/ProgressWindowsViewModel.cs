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

		public delegate void CloseWindowsEventHandler(object sender, EventArgs e);
		public CloseWindowsEventHandler CloseWindowEvent;

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
		public int Progress
		{
			get => _progress;
			set
			{
				_progress = value;
				RaisePropertyChanged(nameof(Progress));
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
		public IProgress<ProgressInfo> TestProgress { get; set; }

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
			Progress = 0;
			Numerator = 0;
			Denominator = 0;

			TestProgress = new Progress<ProgressInfo>(OnProgressChanged);
		}

		public void OnProgressChanged(ProgressInfo progressInfo)
		{
			try
			{
				Title = progressInfo.Title;
				ProcessName = progressInfo.ProcessName;
				Progress = progressInfo.Progress;
				Numerator = progressInfo.Numerator;
				Denominator = progressInfo.Denominator;

				if (100 <= Progress)
				{
					CloseWindowEvent?.Invoke(this, null);
				}
			}
			catch (NullReferenceException)
			{
				//Can the exception ignore...?
			}
		}

		public void OnProgressStart(object sender, EventArgs e)
		{
			AsyncTask.RunTask(TestProgress);
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
			Progress = progressInfo.Progress;
			Numerator = progressInfo.Numerator;
			Denominator = progressInfo.Denominator;

			if (100 <= Progress)
			{
				CloseWindowEvent?.Invoke(this, null);
			}
		}

	}
}
