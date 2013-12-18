using ModelFormatting.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Tests.Tests
{
    [TestFixture]
    public class CultureTests
    {
        private IModelFormatter formatter;

        [TestFixtureSetUp]
        public void Init()
        {
            formatter = new DefaultModelFormatter(new DefaultFormatParser(), new DefaultKeyFormatPrecedenceProvider());
        }

        [Test]
        public void TestCulture()
        {
            var obj = new { Date = new DateTime(2013, 12, 20) };
            var format = formatter.FormatModel(obj, "Date: {Date}");

            // Assert our own culture.
            Assert.AreEqual("Date: 12/20/2013 12:00:00 AM", format);

            // Reformat to ThaiBuddhist
            format = formatter.FormatModel(obj, "Date: {Date}", new CultureInfo("th-TH"));

            // Assert the new Date/Time
            Assert.AreEqual("Date: 20/12/2556 0:00:00", format);
        }
    }
}
