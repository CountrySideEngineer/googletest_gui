using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Model
{
	public interface IAsyncTask<T>
	{
		void RunTask(IProgress<T> progress);
	}
}
