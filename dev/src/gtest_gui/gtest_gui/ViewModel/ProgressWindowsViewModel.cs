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
		}

		/// <summary>
		/// Event handler of progress changed.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="arg">Event argument.</param>
		public void OnProgressChanged(object sender, ProgressChangedCommandArgument arg)
		{
			ProgressInfo progressInfo = arg.ProgressInfo;
			Title = progressInfo.Title;
			ProcessName = progressInfo.ProcessName;
			Progress = progressInfo.Progress;
			Numerator = progressInfo.Numerator;
			Denominator = progressInfo.Denominator;
		}
	}
}
