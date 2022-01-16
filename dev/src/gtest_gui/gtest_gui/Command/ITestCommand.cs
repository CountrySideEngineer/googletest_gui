using gtest_gui.Command.Argument;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command
{
	public interface ITestCommand
	{
		object ExecuteCommand(TestCommandArgument cmdArgument);
	}
}
