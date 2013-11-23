using AutoMapper;
using ModelFormatting.Services;
using ModelFormatting.WebTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelFormatting.WebTests.Controllers
{
    public class MainController : Controller
    {
        private readonly IModelFormatter modelFormatter;

        public MainController(IModelFormatter _modelFormatter)
        {
            modelFormatter = _modelFormatter;
        }

        public ActionResult Index()
        {
            // Gather Object somehow, probably with EF.
            var model = new SampleObject()
            {
                Name = "Jerry",
                Money = 23.45m,
                Age = 21,
                SomeDouble = 25.469d
            };

            // Map it over to our model with formatted annotations.
            var formatmodel = Mapper.Map<SampleObjectModel>(model);

            // Grab the formatted string.
            var formatted = modelFormatter.FormatModel(formatmodel, "Here is a person named {Name}, " +
                "he has {Money} money, " +
                "is {Age} years old, " +
                "and somedouble is {SomeDouble}");

            return View();
        }
    }
}
