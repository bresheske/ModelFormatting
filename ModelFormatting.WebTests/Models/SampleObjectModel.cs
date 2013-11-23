using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelFormatting.WebTests.Models
{
    public class SampleObjectModel
    {
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "C")]
        public decimal Money { get; set; }

        [DisplayFormat(DataFormatString = "0.00")]
        public int Age { get; set; }

        [DisplayFormat(DataFormatString = "0.0")]
        public double SomeDouble { get; set; }
    }
}