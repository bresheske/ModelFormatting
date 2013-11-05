﻿using System;
using ModelFormatting.Extensions.FormattingExtensions;
using ModelFormatting.Models;
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

        [Test]
        public void ReflectiveFormatting()
        {
            var obj = new
                        {
                            Name10 = "Bobby",
                            String = "Bananas",
                        };

            Assert.AreEqual("Name10: Bobby, String: Bananas", 
                obj.FormatModelReflective());
            Assert.AreEqual("<Name10>Bobby</Name10><String>Bananas</String>", 
                obj.FormatModelReflective("<{Key}>{Value}</{Key}>"));
            Assert.AreEqual("<Name10>Bobby</Name10><br /><String>Bananas</String>",
                obj.FormatModelReflective("<{Key}>{Value}</{Key}>", "<br />"));
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
                obj.FormatModel("Name: {Name} BirthDate: {BirthDate} Age: {Age}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.0000",
                obj.FormatModel("Name: {Name} BirthDate: {BirthDate} Age: {Age:0.0000}"));

            Assert.AreEqual("Name: Bobby, BirthDate: 4/3/1987, Age: 25.00",
                obj.FormatModelReflective());
            Assert.AreEqual("<Name>Bobby</Name><BirthDate>4/3/1987</BirthDate><Age>25.00</Age>",
                obj.FormatModelReflective("<{Key}>{Value}</{Key}>"));
            Assert.AreEqual("<Name>Bobby</Name><br /><BirthDate>4/3/1987</BirthDate><br /><Age>25.00</Age>",
                obj.FormatModelReflective("<{Key}>{Value}</{Key}>", "<br />"));
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
                obj.FormatModel("Name: {Name} BirthDate: {BirthDate} Age: {Age} {Oranges}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00 {{Oranges}}",
                obj.FormatModel("Name: {Name} BirthDate: {BirthDate} Age: {Age} {{Oranges}}"));
            Assert.AreEqual("Name: Bobby BirthDate: 4/3/1987 Age: 25.00 {SomeJsonObject: 'Organic'}",
                obj.FormatModel("Name: {Name} BirthDate: {BirthDate} Age: {Age} {SomeJsonObject: 'Organic'}"));
        }
    }
}
