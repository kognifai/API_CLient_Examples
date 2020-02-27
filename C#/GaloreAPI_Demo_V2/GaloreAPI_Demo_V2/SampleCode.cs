using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaloreAPIDemoV2
{
    public class SampleCode
    {
        public async Task Do()
        {
            var client = new GaloreClient(new GaloreConnector());

            var assets = await client.GetAssets("/fleet", null, 1);
            var fleet = assets.First();
            foreach(var edge in fleet.Edges)
            {
                var childNode = edge.Node;
            }

            // Get all timeseries nodes under ship
            var shipTimeseriesNodes = await client.GetAssets("/fleet/ship_apples", "TimeSeries");

            // Warning: should probably not do this :)
            var allTimeseriesNodes = await client.GetAssets(null, "TimeSeries");

            // Fetch latest datapoint for specific timeseries ID
            var lastDataForId = await client.GetLatestTimeseries("123");

            // Get latest data for all specified timeseries ID's
            var latestTimeseriesDataForIds = await client.GetLatestTimeseriesForIds(shipTimeseriesNodes.Select(x => x.TimeseriesId).ToArray());
        }
    }
}
