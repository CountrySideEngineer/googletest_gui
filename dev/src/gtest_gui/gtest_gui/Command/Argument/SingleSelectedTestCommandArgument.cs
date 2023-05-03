using gtest_gui.Model;
using gtest2html;
using System;
using System.Collections.Generic;
using System.Text;

namespace gtest_gui.Command.Argument
{
    public class SingleSelectedTestCommandArgument : TestCommandArgument
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SingleSelectedTestCommandArgument() : base() { }

        /// <summary>
        /// Constructor with TestInformation object.
        /// </summary>
        /// <param name="testInfo">Test information.</param>
        public SingleSelectedTestCommandArgument(TestInformation testInfo) : base(testInfo) { }

        /// <summary>
        /// Constructor with TestCase data.
        /// </summary>
        /// <param name="testCase"></param>
        public SingleSelectedTestCommandArgument(TestCase testCase) : base(testCase) { }

        /// <summary>
        /// Test item id value.
        /// </summary>
        public int TestItemId { get; set; }
    }
}
