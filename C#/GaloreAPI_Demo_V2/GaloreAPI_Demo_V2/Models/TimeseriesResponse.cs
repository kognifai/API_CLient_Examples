using System.Collections.Generic;

namespace GaloreAPIDemoV2.Models
{
    public class TimeseriesResponse
    {
        public List<TimeseriesMetadata>? Metadata { get; set; }

        public List<DataPoint>? DataPoints { get; set; }
    }
}
