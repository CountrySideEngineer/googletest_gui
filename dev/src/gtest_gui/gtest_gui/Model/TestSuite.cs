using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace gtest2html
{
	[XmlRoot("testsuites")]
	public class TestSuites
	{
		[XmlAttribute("tests")]
		public int Tests { get; set; }

		[XmlAttribute("failures")]
		public int Failures { get; set; }

		[XmlAttribute("disabled")]
		public int Disabled { get; set; }

		[XmlAttribute("errors")]
		public int Errors { get; set; }

		[XmlAttribute("time")]
		public float Time { get; set; }

		[XmlAttribute("timestamp")]
		public DateTime TimeStamp { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("testsuite")]
		public List<TestSuite> TestItems { get; set; }

		/// <summary>
		/// Test name.
		/// </summary>
		public string TestName { get; set; }

		/// <summary>
		/// Test suite xml file path.
		/// </summary>
		public string FilePath { get; set; }

		/// <summary>
		/// Test data xml file name.
		/// </summary>
		public string HtmlFileName { 
			get
			{
				return TestName + ".html";
			}
		}
	}

	[XmlRoot("testsuite")]
	public class TestSuite
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("tests")]
		public int Tests { get; set; }

		[XmlAttribute("failures")]
		public int Failures { get; set; }

		[XmlAttribute("disabled")]
		public int Disabled { get; set; }

		[XmlAttribute("errors")]
		public int Errors { get; set; }

		[XmlAttribute("time")]
		public float Time { get; set; }

		[XmlAttribute("timestamp")]
		public DateTime TimeStamp { get; set; }

		[XmlElement("testcase")]
		public List<TestCase> TestCases { get; set; }
	}

	[XmlRoot("testcase")]
	public class TestCase
	{
		public TestCase()
		{
			Failure = null;
		}

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("status")]
		public string Status { get; set; }

		[XmlAttribute("result")]
		public string Result { get; set; }

		[XmlAttribute("time")]
		public float Time { get; set; }

		[XmlAttribute("timestamp")]
		public DateTime Timestamp { get; set; }

		[XmlAttribute("classname")]
		public string ClassName { get; set; }

		[XmlElement("failure")]
		public Failure Failure { get; set; }

		public string Judge
		{
			get
			{
				if (null == Failure)
				{
					return "OK";
				}
				else
				{
					return "NG";
				}

			}
		}

		public bool IsFail
		{
			get
			{
				if (null == Failure)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}
	}

	[XmlRoot("failure")]
	public class Failure
	{
		[XmlAttribute("message")]
		public string Message { get; set; }
	}
}
