using AutoMapper;
using ModelFormatting.WebTests.Models;

namespace ModelFormatting.WebTests
{
    public class AutoMapperBootstrapper
    {

        public static void Initialise()
        {
            Mapper.CreateMap<SampleObject, SampleObjectModel>();
        }
    }
}