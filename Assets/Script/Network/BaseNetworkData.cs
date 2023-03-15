using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TGA.Network
{
    [Serializable]
    public class BaseNetworkData
    {
        public Guid ID { get; protected set; }

        public DateTime Time { get; protected set; }

        public BaseNetworkData()
        {
            ID = Guid.NewGuid();
            Time = DateTime.Now;
        }

        [JsonConstructor]
        public BaseNetworkData(Guid id, DateTime time)
        {
            ID = id;
            Time = time;
        }

        /// <summary>
        /// Process required data
        /// </summary>
        /// <returns>Task operation</returns>
        public virtual Task ProcessData()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Get data infomation for debuging
        /// </summary>
        /// <returns>Data infomation as formatted text</returns>
        public virtual string GetInfo()
        {
            return $"TYPE = {GetType().Name}\n" +
                   $"ID = {ID}\n" +
                   $"Time = {Time}\n";
        }

        /// <summary>
        /// Convert this data to required format(json)
        /// </summary>
        /// <returns>Data as Json</returns>
        public string Serialize()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.SerializeObject(this, settings);
        }

        /// <summary>
        /// Convert string(json) to class structure
        /// </summary>
        /// <param name="value">Json data</param>
        /// <returns>Data as class structure</returns>
        public static T Deserialize<T>(string value)
        {
            //This setting made JsonConvert.DeserializeObject can convert BaseGameModeActionData to subclass automatically
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}