using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RIC.Models.Redmine
{
    public class Tracker
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class TrackerRoot
    {
        [JsonProperty(PropertyName = "trackers")]
        public List<Tracker> Trackers { get; set; }
        public int FindIdByName(string name)
        {
            var tracker = (from v in Trackers where v.Name == name select v).SingleOrDefault();
            if (tracker == null)
            {
                throw new AppException(1000000, string.Format("Tracker not found. name:{0}", name));
            }
            return tracker.Id;
        }
    }

}
