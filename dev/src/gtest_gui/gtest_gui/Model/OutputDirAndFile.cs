using gtest2html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public string TestExeFileName { get; set; }

		/// <summary>
		/// The time the object created.
		/// </summary>
		public DateTime TestTimeStamp { get; set; }

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
			TestTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="rootDirPath">Root dir path user specified.</param>
		public OutputDirAndFile(string rootDirPath)
		{
			RootDirPath = rootDirPath;
			TestExeFileName = string.Empty;
			TestTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Constructor with argument.
		/// </summary>
		/// <param name="rootDirPath"></param>
		/// <param name="testExeFileName"></param>
		public OutputDirAndFile(string rootDirPath, string testExeFileName)
		{
			RootDirPath = rootDirPath;
			TestExeFileName = testExeFileName;
			TestTimeStamp = DateTime.Now;
		}

		/// <summary>
		/// Setup output directory architecture.
		/// </summary>
		/// <param name="testName">Test name.</param>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="PathTooLongException"></exception>
		/// <exception cref="DirectoryNotFoundException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		public virtual IEnumerable<DirectoryInfo> SetUpTestOutputDirectories(string testName)
		{
			string logDir = LogDirPath();
			string outputDir = OutputDirPath();
			string reportDir = ReportDirPath();

			try
			{
				List<string> paths = new List<string>()
				{
					logDir, outputDir, reportDir
				};
				IEnumerable<DirectoryInfo> dirInfos = SetUpTestOutputDirecotries(paths);

				return dirInfos;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Set up output directory structures.
		/// </summary>
		/// <param name="paths">Collection of directory path to create.</param>
		/// <returns>Collection of <para>DirectoryInfo</para> object created.</returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="PathTooLongException"></exception>
		/// <exception cref="DirectoryNotFoundException"></exception>
		/// <exception cref="NotSupportedException"></exception>
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
		/// <exception cref="NullReferenceException"></exception>
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
		/// Returns log file path.
		/// </summary>
		/// <param name="testCase">Test case data for log.</param>
		/// <returns>Path to log file.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public virtual string LogFilePath(TestCase testCase)
		{
			try
			{
				string testCasePath = testCase.Path;
				string name = Path.GetFileNameWithoutExtension(testCasePath);
				string outputDirPath = OutputDirPath();
				string logPath = $@"{outputDirPath}\{name}.log";

				return logPath;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>
		/// Returns report file name the result of test should be set.
		/// </summary>
		/// <param name="testName">Test name</param>
		/// <returns>Report file name.</returns>
		/// <exception cref="NullReferenceException"></exception>
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
		/// <exception cref="ArgumentNullException"></exception>
		protected virtual string TestLogAndReportName(string testName)
		{
			try
			{
				if ((string.IsNullOrEmpty(testName)) || (string.IsNullOrWhiteSpace(testName)))
				{
					throw new ArgumentException();
				}

				string timeStamp = TimeStamp();
				string fileName = $"{testName}_{timeStamp}";
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
		/// <exception cref="DirectoryNotFoundException"></exception>
		public virtual IEnumerable<string> GetTestReportFiles()
		{
			string testReportDir = ReportDirPath();
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
		/// Returns collection of test log file path.
		/// </summary>
		/// <returns>Collection of test log file path.</returns>
		/// <exception cref="DirectoryNotFoundException"></exception>
		public virtual IEnumerable<string> GetTestLogFiles()
		{
			string testOutputDir = OutputDirPath();
			if (Directory.Exists(testOutputDir))
			{
				string outputFileTemplate = "*.log";
				string[] logFiles = Directory.GetFiles(testOutputDir, outputFileTemplate);
				IEnumerable<string> testLogFiles = new List<string>(logFiles);
				return testLogFiles;
			}
			else
			{
				throw new DirectoryNotFoundException();
			}
		}

		public virtual string GetTestLogFile(string testName)
		{
			string logFileName = TestLogAndReportName(testName);
			IEnumerable<string> logFiles = GetTestLogFiles();
			IEnumerable<string> logFile = logFiles.Where(_ => _.Contains(logFileName));
			string logFileItem = logFile.First();

			return logFileItem;
		}

		/// <summary>
		/// Returns time stamp in string type. 
		/// </summary>
		/// <returns>Time stamp in string with format specified by TimeStampFormat property.</returns>
		/// <exception cref="FormatException"></exception>
		public string TimeStamp()
		{
			try
			{
				string timeStamp = TimeStampWithFormat(TimeStampFormat);
				return timeStamp;
			}
			catch (FormatException)
			{
				throw;
			}
		}

		public string TimeStamp(DateTime dateTime)
		{
			string timeStamp = TimeStampWithFormat(dateTime, TimeStampFormat);
			return timeStamp;
		}

		/// <summary>
		/// Returns time stamp in string type with format.
		/// </summary>
		/// <param name="format">Time stamp format.</param>
		/// <returns>Time stmap in string.</returns>
		/// <exception cref="FormatException"></exception>
		public string TimeStampWithFormat(string format)
		{
			try
			{
				string timeStamp = TimeStampWithFormat(TestTimeStamp, format);
				return timeStamp;
			}
			catch (FormatException)
			{
				throw;
			}
		}

		public string TimeStampWithFormat(DateTime timeStamp, string format)
		{
			try
			{
				string timeStampValue = timeStamp.ToString(format);
				return timeStampValue;
			}
			catch (FormatException)
			{
				throw;
			}
		}
	}
}
