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
        public void CoreEfficiencySpeed()
        {
            // Speed tracking objects.
            var sw = new Stopwatch();
            var formatter = new DefaultModelFormatter(new DefaultFormatParser(), 
                new DefaultKeyFormatPrecedenceProvider(),
                new DefaultPropertyProvider());

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
