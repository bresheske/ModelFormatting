using ModelFormatting.Services;
using ModelFormatting.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFormatting.Tests.Tests
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void CoreActivationPrecedence()
        {
            // The core takes precedence (defaultly) on the attributes over the text
            // formatting. When the core is not activated, the text takes precedence.

            // Arrange
            var formatter = new DefaultModelFormatter();
            var obj = new TestModelWithAttributes { BirthDate = new DateTime(2013, 12, 20), Age = 20, Money = 45.54m };

            // Make sure the core isn't active yet.
            if (Core.IsRegistered<TestModelWithAttributes>())
                Core.ClearModel<TestModelWithAttributes>();

            // Act: Default formatting.
            var formatdefault = formatter.FormatModel(obj, "bd:{BirthDate},a:{Age},m:{Money}");
            // Act: Formatting overrides with text template formats.
            var formattexttemplate = formatter.FormatModel(obj, "bd:{BirthDate:yyyy/MM/dd},a:{Age:0.000},m:{Money:0.0}");
            // Act: Overrides with the core, back to attributes.
            Core.RegisterModel<TestModelWithAttributes>();
            var formatcore = formatter.FormatModel(obj, "bd:{BirthDate},a:{Age},m:{Money}");
            // Act: Attempt to override, should not.
            var formatcoreoverride = formatter.FormatModel(obj, "bd:{BirthDate:yyyy/MM/dd},a:{Age:0.000},m:{Money:0.0}");

            // Asserts
            Assert.AreEqual("bd:12/20/2013,a:20.00,m:$45.54", formatdefault);
            Assert.AreEqual("bd:2013/12/20,a:20.000,m:45.5", formattexttemplate);
            Assert.AreEqual("bd:12/20/2013,a:20.00,m:$45.54", formatcore);
            Assert.AreEqual("bd:12/20/2013,a:20.00,m:$45.54", formatcoreoverride);
        }

        [Test]
        public void CoreEfficiencySpeed()
        {
            // Test to assert the core has a major effect on the runtime speed of the FormatModel
            // operation.

            // Speed tracking objects.
            var sw = new Stopwatch();
            var formatter = new DefaultModelFormatter();

            // Let's double check to make sure the model isn't already in the core.
            if (Core.IsRegistered<TestModelWithAttributes>())
                Core.ClearModel<TestModelWithAttributes>();

            // First, test the speed of some normal formatting. (DateTime seems to take the longest)
            var obj = new TestModelWithAttributes{ BirthDate = new DateTime(2013, 12, 20), Age = 20, Money = 45.54m };
            sw.Start();
            for(int i = 0; i < 200000; i++)
            {
                formatter.FormatModel(obj, "something {BirthDate} something {Money}");
            }
            sw.Stop();
            var timenormal = sw.ElapsedMilliseconds;

            // Now let's setup the core and parse through our type initially.
            ModelFormatting.Core.RegisterModel<TestModelWithAttributes>();

            // Now we test the speed of the core.
            sw.Restart();
            for (int i = 0; i < 200000; i++)
            {
                formatter.FormatModel(obj, "something {BirthDate} something {Money}");
            }
            sw.Stop();
            var timecore = sw.ElapsedMilliseconds;

            // Assert the speed was much faster.
            Debug.WriteLine("NormalSpeed: {0:0.00}s, CoreSpeed: {1:0.00}s", timenormal / 1000f, timecore / 1000f);
            Assert.IsTrue(timenormal > timecore * 2);
        }
    }
}