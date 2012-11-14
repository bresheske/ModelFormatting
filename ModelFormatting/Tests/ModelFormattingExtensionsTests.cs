using System;
using ModelFormatting.Extensions.FormattingExtensions;
using NUnit.Framework;

namespace ModelFormatting.Tests
{
    [TestFixture]
    public class ModelFormattingExtensionsTests
    {

        [Test]
        public void SimpleFormatting()
        {
            var obj = new { Name = "Scott", Address = "101 Elm Street"};
            var format = obj.FormatModel("Name: {Name} Address: {Address}");
            Assert.AreEqual("Name: Scott Address: 101 Elm Street", format);
        }

        [Test]
        public void NumberFormatting()
        {
            var obj = new { Name = "Scott", Money = 2m };
            var format = obj.FormatModel("Name: {Name} Money: {Money:C}");
            Assert.AreEqual("Name: Scott Money: $2.00", format);

            var decobj = new {Name = "Nancy", Number = 2000};
            Assert.AreEqual("Name: Nancy Number: 2000.00", 
                decobj.FormatModel("Name: {Name} Number: {Number:F}"));
            Assert.AreEqual("Name: Nancy Number: 7D0",
                decobj.FormatModel("Name: {Name} Number: {Number:X}"));
            Assert.AreEqual("Name: Nancy Number: 2000.0",
                decobj.FormatModel("Name: {Name} Number: {Number:0.0}"));
            Assert.AreEqual("Name: Nancy Number: (2000)",
                decobj.FormatModel("Name: {Name} Number: {Number:(#).##}"));
            Assert.AreEqual("Name: Nancy Number: (2000).00",
                decobj.FormatModel("Name: {Name} Number: {Number:(#).00}"));
            Assert.AreEqual("Name: Nancy Number: 200000%",
                decobj.FormatModel("Name: {Name} Number: {Number:0%}"));
        }

        [Test]
        public void DateFormatting()
        {
            var obj = new { Name = "Scott", Date = new DateTime(2012, 11, 13) };
            Assert.AreEqual("Name: Scott Date: 11/13/2012", 
                obj.FormatModel("Name: {Name} Date: {Date:d}"));
            Assert.AreEqual("Name: Scott Date: Tuesday, November 13, 2012",
                obj.FormatModel("Name: {Name} Date: {Date:D}"));
        }

        [Test]
        public void Robustness()
        {
            var obj = new
                          {
                              Name10 = "$_()/@1",
                              Date = new DateTime(2012, 11, 13),
                              Money = 24.54m,
                              Ticks = 1504388483838394594L,
                              SomeInt = 12
                          };
            Assert.AreEqual("$_()/@1 11/13/2012 $24.54 1504388483838394594.00 12", 
                obj.FormatModel("{Name10} {Date:d} {Money:C} {Ticks:0.00} {SomeInt}"));
        }
    }
}
