using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gtest_gui.Model
{
	public class OutputDirAndFile
	{
		/// <summary>
		/// Output root directory path
		/// </summary>
		public string RootDirPath { get; protected set; }

		/// <summary>
		/// Test execute file name.
		/// </summary>
		public string TestExeFileName { get; protected set; }

		/// <summary>
		/// The time the object created.
		/// </summary>
		protected DateTime _newedTimeStamp;

		/// <summary>
		/// Default time stamp format.
		/// </summary>
		public string TimeStampFormat { get; set; } = "yyyyMMddHHmmss";

		/// <summary>
		/// Default constructor.
		/// </summary>
		public OutputDirAndFile()
		{
			RootDirPath = Directory.GetCurrentDirectory();
			TestExeFileName = string.Empty;
			_newedTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="rootDirPath">Root dir path user specified.</param>
		public OutputDirAndFile(string rootDirPath)
		{
			RootDirPath = rootDirPath;
			TestExeFileName = string.Empty;
			_newedTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="rootDirPath"></param>
		/// <param name="testExeFileName"></param>
		public OutputDirAndFile(string rootDirPath, string testExeFileName)
		{
			this.RootDirPath = rootDirPath;
			this.TestExeFileName = testExeFileName;
			_newedTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Setup output directory architecture.
		/// </summary>
		/// <param name="testName">Test name.</param>
		public virtual IEnumerable<DirectoryInfo> SetUpTestOutputDirecries(string testName)
		{
			List<string> paths = new List<string>()
			{
				LogDirPath(),
				OutputDirPath(),
				ReportDirPath()
			};
			IEnumerable<DirectoryInfo> dirInfos = SetUpTestOutputDirecotries(paths);

			return dirInfos;
		}

		/// <summary>
		/// Set up output directory structures.
		/// </summary>
		/// <param name="paths">Collection of directory path to create.</param>
		/// <returns>Collection of <para>DirectoryInfo</para> object created.</returns>
		protected virtual IEnumerable<DirectoryInfo> SetUpTestOutputDirecotries(IEnumerable<string> paths)
		{
			try
			{
				List<DirectoryInfo> dirInfos = new List<DirectoryInfo>();
				foreach (var pathItem in paths)
				{
					DirectoryInfo dirInfo = Directory.CreateDirectory(pathItem);
					dirInfos.Add(dirInfo);
				}
				return dirInfos;
			}
			catch (Exception ex)
			when ((ex is NullReferenceException)
				|| (ex is ArgumentNullException))
			{
				//In this case, the code which calls this method is invalid.
				throw;
			}
			catch (Exception ex)
			when ((ex is IOException)
				|| (ex is UnauthorizedAccessException)
				|| (ex is ArgumentException) 
				|| (ex is PathTooLongException) 
				|| (ex is DirectoryNotFoundException) 
				|| (ex is NotSupportedException))
			{
				throw;
			}
		}

		/// <summary>
		/// Create log dir path.
		/// </summary>
		/// <returns></returns>
		public virtual string LogDirPath()
		{
			string logDirPath = $@"{RootDirPath}\log";
			if ((!string.IsNullOrEmpty(TestExeFileName)) &&
				(!string.IsNullOrWhiteSpace(TestExeFileName)))
			{
				logDirPath = $@"{logDirPath}\{TestExeFileName}";
			}
			return logDirPath;
		}

		/// <summary>
		/// Create output directory path.
		/// </summary>
		/// <returns>Output directory path.</returns>
		public virtual string OutputDirPath()
		{
			string logDirPath = LogDirPath();
			string outputDirPath = $@"{logDirPath}\output";
			return outputDirPath;
		}

		/// <summary>
		/// Create report directory path.
		/// </summary>
		/// <returns>Repotr directory path.</returns>
		public virtual string ReportDirPath()
		{
			string logDirPath = LogDirPath();
			string reportDirPath = $@"{logDirPath}\report";
			return reportDirPath;
		}

		/// <summary>
		/// Returns log file name the test should log.
		/// </summary>
		/// <param name="testName">Test Name</param>
		/// <returns>Log file name.</returns>
		public virtual string LogFilePath(string testName)
		{
			try
			{
				string logFileName = TestLogAndReportName(testName);
				string logFileNameWithExt = $"{logFileName}.log";
				string logFilePath = $@"{OutputDirPath()}\{logFileNameWithExt}";
				return logFilePath;
			}
			catch (NullReferenceException)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns report file name the result of test should be set.
		/// </summary>
		/// <param name="testName">Test name</param>
		/// <returns>Report file name.</returns>
		public virtual string TestReportFilePath(string testName)
		{
			try
			{
				string reportFileName = TestLogAndReportName(testName);
				string reportFileNameWithExt = $@"{reportFileName}.xml";
				string reportFilePath = $@"{ReportDirPath()}\{reportFileNameWithExt}";
				return reportFilePath;
			}
			catch (NullReferenceException)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns log and report file name the test should create.
		/// </summary>
		/// <param name="testName">Test name.</param>
		/// <returns>Log and report file name.</returns>
		/// <exception cref="NullReferenceException">Implementation imvalid.</exception>
		protected virtual string TestLogAndReportName(string testName)
		{
			try
			{
				var dateTimeNow = TimeStamp();
				string fileName = $"{testName}_{dateTimeNow}";
				return fileName;
			}
			catch (NullReferenceException)
			{
				/*
				 * If the exception occurred, it means that the implementation is invalid.
				 * So, it is not needed to handle the exception and notify it to user.
				 */
				throw;
			}
		}

		/// <summary>
		/// Returns collection of test report file path.
		/// </summary>
		/// <returns>Collection of test report file path.</returns>
		public virtual IEnumerable<string> GetTestReportFiles()
		{
			var testReportDir = this.ReportDirPath();
			if (Directory.Exists(testReportDir))
			{
				string repotrFileTemplate = "*.xml";
				string[] reportFiles = Directory.GetFiles(testReportDir, repotrFileTemplate);
				IEnumerable<string> testReportFiles = new List<string>(reportFiles);
				return testReportFiles;
			}
			else
			{
				throw new DirectoryNotFoundException();
			}
		}

		/// <summary>
		/// Returns time stamp in string type. 
		/// </summary>
		/// <returns>Time stamp in string with format specified by TimeStampFormat property.</returns>
		public string TimeStamp()
		{
			var timeStamp = TimeStampWithFormat(TimeStampFormat);
			return timeStamp;
		}

		/// <summary>
		/// Returns time stamp in string type with format.
		/// </summary>
		/// <param name="format">Time stamp format.</param>
		/// <returns>Time stmap in string.</returns>
		public string TimeStampWithFormat(string format)
		{
			try
			{
				var timeStamp = _newedTimeStamp.ToString(format);
				return timeStamp;
			}
			catch (FormatException)
			{
				throw;
			}
		}
	}
}
