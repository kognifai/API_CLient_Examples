using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GaloreAPIDemoV2.Models
{
    [DataContract]
    public class DataPoint
    {
        public string? Timestamp { get; set; }

        public List<double>? Values { get; set; }

        public override string ToString()
        {
            var values = string.Join(", ", Values.Select(x => x.ToString()));
            return $"Timestamp: {Timestamp} Value(s): {values}";
        }
    }
}