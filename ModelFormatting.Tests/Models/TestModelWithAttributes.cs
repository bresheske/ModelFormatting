using System;
using System.ComponentModel.DataAnnotations;

namespace ModelFormatting.Tests.Models
{
    public class TestModelWithAttributes
    {
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "d")]
        public DateTime BirthDate { get; set; }

        [DisplayFormat(DataFormatString = "0.00")]
        public int Age { get; set; }
    }
}
