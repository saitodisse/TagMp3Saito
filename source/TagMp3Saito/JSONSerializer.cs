using System.Web.Script.Serialization;

namespace TagMp3Saito
{
    public static class JSONSerializer
    {
        public static string JSonSerialize<T>(T obj)
        {
            var jser = new JavaScriptSerializer();
            return jser.Serialize(obj);
        }

        public static T JSonDeserialize<T>(string json)
        {
            var jser = new JavaScriptSerializer();
            return jser.Deserialize<T>(json);
        }

    }
}
