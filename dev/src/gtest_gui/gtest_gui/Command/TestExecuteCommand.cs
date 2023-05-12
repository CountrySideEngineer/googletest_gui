using gtest_gui.Command.Argument;
using gtest_gui.Command.Exception;
using gtest_gui.Model;
using gtest_gui.MoveWindow;
using gtest_gui.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace gtest_gui.Command
{
	/// <summary>
	/// Command class to execute test
	/// </summary>
	public class TestExecuteCommand : ITestCommand
	{
		/// <summary>
		/// Field of TestRunner object to run test.
		/// </summary>
		protected TestRunner _runner;

		protected OutputLogBuilder _logBuilder;

		public OutputDirAndFile OutputDirInfo { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TestExecuteCommand()
		{
			_runner = new GoogleTestRunner();
			_logBuilder = new OutputLogBuilder();
			OutputDirInfo = new OutputDirAndFile();
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="runner">TestRunner object to run test.</param>
		public TestExecuteCommand(TestRunner runner)
		{
			_runner = runner;
			_logBuilder = new OutputLogBuilder();
			OutputDirInfo = new OutputDirAndFile();
		}

		/// <summary>
		/// Constructor with arguments.
		/// </summary>
		/// <param name="runner">TestRunner object to run test.</param>
		/// <param name="outputDirInfo">OutputDirAndFile object includes output directory and file data.</param>
		public TestExecuteCommand(TestRunner runner, OutputDirAndFile outputDirInfo)
		{
			_runner = runner;
			_logBuilder = new OutputLogBuilder();
			OutputDirInfo = outputDirInfo;
		}

		/// <summary>
		/// Execute test.
		/// </summary>
		/// <param name="cmdArgument">Argument for test including target test file and information to run it.</param>
		/// <returns>Returns always 0.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		/// <exception cref="NullReferenceException">Invalid exception.</exception>
		public virtual object ExecuteCommand(TestCommandArgument cmdArgument)
		{
			try
			{
				SetUpTestRunner(cmdArgument);

				TestInformation testInfo = cmdArgument.TestInfo;
				_runner.Run(testInfo);

				TearDownTestRunner();

				return 0;
			}
			catch (System.Exception ex)
			when ((ex is ArgumentException) ||
				(ex is UnauthorizedAccessException) ||
				(ex is NotSupportedException))
			{
				var exception = new CommandException()
				{
					Code = 0x00000001,
					Title = "テスト実行エラー",
					Summary = "選択されたテストが実行できませんでした。" + Environment.NewLine +
						"実行ファイルの有無、指定されたテストがGoogletestを使用しているか、確認してください。"
				};
				throw exception;
			}
			catch (NullReferenceException)
			{
				throw;
			}
		}

		/// <summary>
		/// Set up TestRunner object with parameters passed with argument, TestCommandArgument.
		/// </summary>
		/// <param name="cmdArg">Command argument used to setup TestRunner object.</param>
		/// <returns>TestRunner object to run test.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		protected virtual void SetUpTestRunner(TestCommandArgument cmdArg)
		{
			try
			{
				string testFilePath = cmdArg.TestInfo.TestFile;
				string testFileName = System.IO.Path.GetFileNameWithoutExtension(testFilePath);
				OutputDirInfo.TestExeFileName = testFileName;
				_logBuilder.OutputDirFile = OutputDirInfo;

				_runner.Target = testFilePath;
				_runner.OutputDirFile = OutputDirInfo;
				_runner.TestDataReceivedEventHandler += _logBuilder.OnDataReceived;
				_runner.TestDataFinisedEventHandler += _logBuilder.OnDataReceiveFinished;
			}
			catch (System.Exception ex)
			when ((ex is ArgumentException) ||
				(ex is UnauthorizedAccessException) ||
				(ex is NotSupportedException))
			{
				throw ex;
			}
			catch (NullReferenceException)
			{
				throw;
			}
		}

		/// <summary>
		/// Tear down the test.
		/// </summary>
		protected virtual void TearDownTestRunner()
		{
			_runner.TestDataReceivedEventHandler -= _logBuilder.OnDataReceived;
			_runner.TestDataFinisedEventHandler -= _logBuilder.OnDataReceiveFinished;
		}
	}
}
