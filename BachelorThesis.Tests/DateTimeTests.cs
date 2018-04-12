using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business.Parsers;
using NUnit.Framework;

namespace BachelorThesis.Tests
{
    [TestFixture]
    public class DateTimeTests
    {
        [Test]
        [TestCase("01-02-2018 09:00:00")]
        [TestCase("01-02-2018 09:01:40")]
        [TestCase("03-02-2018 08:31:00")]
        [TestCase("01-02-2018 15:34:23")]
        public void DateTime_Parse_ShouldReturnCorrectTime(string input)
        {
            Assert.DoesNotThrow(() =>
            {
                var dateTime = DateTime.ParseExact(input, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);


            });
        }
    }
}
