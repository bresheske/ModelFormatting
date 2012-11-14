using System.Web.Script.Serialization;

namespace ModelFormatting.Extensions.JsonExtensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static T FromJson<T>(this string obj)
        {
            return new JavaScriptSerializer().Deserialize<T>(obj);
        }
    }
}
