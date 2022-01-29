using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.MoveWindow
{
    public class Move2TestProgress : Move2Progress
    {
		public override void Move(object srcContext)
		{
			var testRunner = (TestRunner)srcContext;

		}
	}
}
