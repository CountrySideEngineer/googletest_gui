using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace gtest_gui.Model
{
	public class OutputLogBuilder
	{
		/// <summary>
		/// Collection of log.
		/// </summary>
		protected List<string> _logCollection;

		public OutputDirAndFile OutputDirFile { get; set; }

		/// <summary>
		/// Default collection
		/// </summary>
		public OutputLogBuilder()
		{
			_logCollection = null;
		}

		/// <summary>
		/// Constructor with argument
		/// </summary>
		/// <param name="outputDirFile">Directory and file information to output log.</param>
		public OutputLogBuilder(OutputDirAndFile outputDirFile)
		{
			OutputDirFile = outputDirFile;
		}

		/// <summary>
		/// Data received event handler
		/// </summary>
		/// <param name="sender">event sender object.</param>
		/// <param name="e">Received data.</param>
		public void OnDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (null != e.Data)
			{
				Append(e.Data);
			}
		}

		/// <summary>
		/// Date receive finished event handler.
		/// </summary>
		/// <param name="sender">Event sender object.</param>
		/// <param name="e">Finished receiving data event argument.</param>
		public void OnDataReceiveFinished(object sender, TestDataFinishedEventArgs e)
		{
			TestItem testItem = e.TestItem;
			FlushOutput(testItem.Name);
		}

		/// <summary>
		/// Flush received data into a file.
		/// </summary>
		/// <param name="testItem">Test name.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="NullReferenceException"></exception>
		protected void FlushOutput(string testName)
		{
			if ((string.IsNullOrEmpty(testName)) || (string.IsNullOrWhiteSpace(testName)))
			{
				throw new ArgumentException();
			}
			try
			{
				string logFilePath = OutputDirFile.LogFilePath(testName);
				using (var writer = new StreamWriter(logFilePath))
				{
					writer.Write(ToString());
				}
				_logCollection.Clear();
			}
			catch (NullReferenceException)
			{
				throw;
			}
		}

		/// <summary>
		/// Append data to log collection.
		/// </summary>
		/// <param name="output">Log data to set.</param>
		public void Append(string output)
		{
			if (null == _logCollection)
			{
				_logCollection = new List<string>();
				_logCollection.Clear();
			}
			_logCollection.Add(output);
		}

		/// <summary>
		/// Convert collection of log to string data type value.
		/// Each item in collection corresponds a line in returned value.
		/// </summary>
		/// <returns>Log data.</returns>
		public override string ToString()
		{
			string toString = string.Empty;
			foreach (var item in _logCollection)
			{
				toString += item;
				toString += "\r\n";
			}
			return toString;
		}
	}
}
