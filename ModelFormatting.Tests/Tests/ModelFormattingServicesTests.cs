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
            var obj = new { Name = "Scott", Address = "101 Elm Street" };
            var format = formatter.FormatModel(obj, "Name: {Name} Address: {Address}");
            Assert.AreEqual("Name: Scott Address: 101 Elm Street", format);
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
