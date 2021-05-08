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

		public string TestName { get; set; }

		public string HtmlFileName { 
			get
			{
				return this.TestName + ".html";
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
			this.Failure = null;
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
				if (null == this.Failure)
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
				if (null == this.Failure)
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
