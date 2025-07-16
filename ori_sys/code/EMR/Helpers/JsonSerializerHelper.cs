using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


namespace Silverdraw.Client
{
 
    public static class JsonSerializerHelper
    {
        /// <summary>
        /// Adds an extension method to a string
        /// </summary>
        /// <typeparam name="T">The expected type</typeparam>
        /// <param name="json">Json string data</param>
        /// <returns>The deserialized object graph</returns>
        public static T JsonDeserialize<T>(this string json)
        {
            using (MemoryStream mstream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(mstream);
            }
        }

        /// <summary>
        /// Serialize the object to Json string
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized string</returns>
        public static string JsonSerialize(this object obj)
        {
            using (MemoryStream mstream = new MemoryStream())
            {
                DataContractJsonSerializer serializer =
                        new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(mstream, obj);
                mstream.Position = 0;

                using (StreamReader reader = new StreamReader(mstream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
