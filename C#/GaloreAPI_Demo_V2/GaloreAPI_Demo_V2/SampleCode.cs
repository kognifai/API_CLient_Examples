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

            var assets = await client.GetAssets("/Fleet", null, 1);
            var fleet = assets.First();
            foreach(var edge in fleet.Edges)
            {
                var childNode = edge.Node;
            }


            var fleetTimeseriesNodes = await client.GetAssets("/Fleet/Tank", "TimeSeries");

            var allTimeseriesNodes = await client.GetAssets(null, "TimeSeries");


            var lastDataForId = await client.GetLatestTimeseries("123");

            var latestTimeseriesDataForIds = await client.GetLatestTimeseriesForIds(fleetTimeseriesNodes.Select(x => x.TimeseriesId).ToArray());
        }
    }
}
