using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace gtest_gui.Model
{
	public class GoogleTestListReader : TestListReader
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public GoogleTestListReader() : base() { }

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="path">Path to file to get the list of test to execute.</param>
		public GoogleTestListReader(string path)  :  base(path) { }

		/// <summary>
		/// Get ProcessStartInfo object to be used in process to read process from application using googletest framework.
		/// </summary>
		/// <param name="fileName">Path to file to run.</param>
		/// <returns>ProcessStartInfo object</returns>
		protected override ProcessStartInfo CreateProcStartInfo(string fileName)
		{
			try
			{
				var info = base.CreateProcStartInfo(fileName);
				info.Arguments = "--gtest_list_tests";

				return info;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
