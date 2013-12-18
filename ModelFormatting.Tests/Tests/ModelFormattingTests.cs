using ModelFormatting.Tests.Models;
using ModelFormatting.Services;
using NUnit.Framework;
using System;

namespace ModelFormatting.Tests
{
    [TestFixture]
    public class ModelFormattingTests
    {
        private IModelFormatter formatter;

        [TestFixtureSetUp]
        public void Init()
        {
            formatter = new DefaultModelFormatter(new DefaultFormatParser(), new DefaultKeyFormatPrecedenceProvider());
        }

        [Test]
        public void SimpleFormatting()
        {
            var obj = new { Name = "Scott", Address = "101 Elm Street"};
            var format = formatter.FormatModel(obj, "Name: {Name} Address: {Address}");
            Assert.AreEqual("Name: Scott Address: 101 Elm Street", format);
        }

        [Test]
        public void NumberFormatting()
        {
            var obj = new { Name = "Scott", Money = 2m };
            var format = formatter.FormatModel(obj, "Name: {Name} Money: {Money:C}");
            Assert.AreEqual("Name: Scott Money: $2.00", format);

            var decobj = new {Name = "Nancy", Number = 2000};
            Assert.AreEqual("Name: Nancy Number: 2000.00",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:F}"));
            Assert.AreEqual("Name: Nancy Number: 7D0",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:X}"));
            Assert.AreEqual("Name: Nancy Number: 2000.0",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:0.0}"));
            Assert.AreEqual("Name: Nancy Number: (2000)",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:(#).##}"));
            Assert.AreEqual("Name: Nancy Number: (2000).00",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:(#).00}"));
            Assert.AreEqual("Name: Nancy Number: 200000%",
                formatter.FormatModel(decobj, "Name: {Name} Number: {Number:0%}"));
        }

        [Test]
        public void DateFormatting()
        {
            var obj = new { Name = "Scott", Date = new DateTime(2012, 11, 13) };
            Assert.AreEqual("Name: Scott Date: 11/13/2012",
                formatter.FormatModel(obj, "Name: {Name} Date: {Date:d}"));
            Assert.AreEqual("Name: Scott Date: Tuesday, November 13, 2012",
                formatter.FormatModel(obj, "Name: {Name} Date: {Date:D}"));
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
                formatter.FormatModel(obj, "{Name10} {Date:d} {Money:C} {Ticks:0.00} {SomeInt}"));
        }

        [Test]
        public void ReflectiveDataAnnotation()
        {
            var obj = new TestModelWithAttributes()
            {
                Age = 25,
                BirthDate = new DateTime(1987, 04, 03),
                Name = "Bobby"
            };

            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00",
                formatter.FormatModel(obj, "Name: {Name} BirthDate: {BirthDate} Age: {Age}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.0000",
                formatter.FormatModel(obj, "Name: {Name} BirthDate: {BirthDate} Age: {Age:0.0000}"));
        }

        [Test]
        public void ReservedCharacters()
        {
            var obj = new TestModelWithAttributes()
            {
                Age = 25,
                BirthDate = new DateTime(1987, 04, 03),
                Name = "Bobby"
            };

            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00 {Oranges}",
                formatter.FormatModel(obj, "Name: {Name} BirthDate: {BirthDate} Age: {Age} {Oranges}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00 {{Oranges}}",
                formatter.FormatModel(obj, "Name: {Name} BirthDate: {BirthDate} Age: {Age} {{Oranges}}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00 {SomeJsonObject: 'Organic'}",
                formatter.FormatModel(obj, "Name: {Name} BirthDate: {BirthDate} Age: {Age} {SomeJsonObject: 'Organic'}"));
        }
    }
}
