using ModelFormatting.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Tests.Tests
{
    [TestFixture]
    public class ModelFormattingServicesTests
    {

        [Test, TestCaseSource("IOCInjecter")]
        public void IOCLayer(IModelFormatter formatter)
        {
            var obj = new { Name = "Scott", Address = "101 Elm Street", Money = 10.10m };
            var format = formatter.FormatModel(obj, "Name: {Name}, Address: {Address}");
            Assert.AreEqual("Name: Scott, Address: 101 Elm Street", format);
            format = formatter.FormatModel(obj, "Name: {Name}, Money: {Money:C}");
            Assert.AreEqual("Name: Scott, Money: $10.10", format);
        }

        private object[] IOCInjecter()
        {
            return new object[]
            {
                new DefaultModelFormatter()
            };
        }
    }
}
