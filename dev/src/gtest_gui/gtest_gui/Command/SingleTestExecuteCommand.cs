using gtest_gui.Command.Argument;
using gtest_gui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gtest_gui.Command
{
    public class SingleTestExecuteCommand : TestExecuteCommand
    {
		public override object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			try
			{
				SingleSelectedTestCommandArgument arg = (SingleSelectedTestCommandArgument)cmdArgument;
				TestInformation testInfo = new TestInformation(arg.TestInfo);
				foreach (var item in testInfo.TestItems)
				{
					item.IsSelected = false;
				}
				testInfo.TestItems.ElementAt(arg.TestItemId).IsSelected = true;
				TestCommandArgument cmdArg = new TestCommandArgument(testInfo);

				return base.ExecuteCommand(cmdArg);
			}
			catch (System.Exception ex)
			when ((ex is InvalidCastException) ||
				(ex is ArgumentOutOfRangeException) ||
				(ex is ArgumentNullException))
			{
				throw ex;
			}
		}
	}
}
