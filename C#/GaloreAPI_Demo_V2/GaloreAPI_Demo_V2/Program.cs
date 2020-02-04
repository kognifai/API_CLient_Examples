using GaloreAPIDemoV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GaloreAPIDemoV2
{
    static class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAll(Demo());
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static async Task Demo()
        {
            try
            {
                Console.WriteLine("- - - - - - - Galore API V2 Demo - - - - - - -");

                var client = new GaloreClient(new GaloreConnector());

                Console.Write("Fetching fleet...");
                var fleet = await client.GetAssetByPath("/Fleet", 1);
                Console.Write(" Success!\n");
                Console.WriteLine(FormatNode(fleet));

                Console.WriteLine($"\n{fleet.Name} edges:");
                foreach(var edge in fleet.Edges)
                {
                    Console.WriteLine($"\tName: {edge.Name} ID: {edge.Node.Id}");
                }

                Console.Write("\nFetching ship by ID...");
                var shipEdge = fleet.Edges.Find(x => x.Name.ToLower() == "ship1");
                if (shipEdge == null)
                {
                    Console.Write(" Failed.");
                    return;
                }
                var ship = await client.GetAssetById(shipEdge.Node.Id, 1);
                Console.Write(" Success!\n");

                var tank = ship.Edges.Find(x => x.Name.ToLower() == "tank1");
                var timeseriesNodes = await client.GetAssets(path: tank.Node.Path, nodeType: "TimeSeries");
                Console.Write("Fetching latest data for timeseries nodes...");
                var latestDataForNodes = await client.GetLatestTimeseriesForIds(timeseriesNodes.Select(x => x.Id).ToArray());
                Console.Write(" Success!\n");
                Console.WriteLine("\nLatest data for nodes:");
                foreach (var (key, value) in latestDataForNodes)
                {
                    if (string.IsNullOrEmpty(value.Error))
                    {
                        Console.WriteLine($"\t{key}: {value.DataPoint}");
                    }
                    else
                    {
                        Console.WriteLine($"\t{key}: {value.Error}");
                    }
                }

                var tsNode = timeseriesNodes.Find(x => x.Name.ToLower() == "random");
                Console.WriteLine("\nFetching data from node 'Random'...");

                var from = new DateTime(2019, 12, 11);
                var to = new DateTime(2020, 12, 11);
                var data = await client.GetTimeseriesData(tsNode!.TimeseriesId, from, to);
                Console.WriteLine();
                PrintData(data, 15);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }        

        static void PrintData(TimeseriesResponse timeseriesResponse, int rowCount)
        {
            var columns = new string[] { "Timestamp" }.Concat(timeseriesResponse.Metadata.Select(md => $"{md.Name} ({md.Unit})")).ToArray();
            var data = timeseriesResponse.DataPoints.Select(x =>
            {
                var arr = new object[] { x.Timestamp ?? "-" };
                return arr.Concat(x.Values.Select(val => Math.Round(val, 5).ToString(CultureInfo.InvariantCulture))).ToArray();
            }).Take(rowCount).ToArray();

            Console.WriteLine(FormatData(columns, data));
        }

        static string FillLength(this string s, int length, char fillChar = ' ')
        {
            var missingCount = length - s.Length;
            return missingCount > 0 ? s + new string(fillChar, missingCount) : s;
        }

        static string FormatData(string[] columns, object[][] data)
        {
            var columnLengths = columns.Select((col, i) =>
            {
                var length = data.Max(x => x[i]?.ToString()?.Length ?? 0);
                return Math.Max(length, col.Length);
            }).ToList();

            var header = string.Join(" | ", columns.Select((col, i) => col.FillLength(columnLengths[i])));
            var spacer = "".FillLength(header.Length, '-');
            var text = $"{spacer}\n{header}\n{spacer}";
            foreach(var row in data)
            {
                var rowText = string.Join(" | ", row.Select((value, i) => (value?.ToString() ?? "").FillLength(columnLengths[i])));
                text += "\n" + rowText;
            }
            return text;
        }

        static string FormatNode(AssetNodeModel node)
        {
            return $"ID: {node.Id}\nName: {node.Name}\nDisplay Name: {node.DisplayName}\nPath: {node.Path}";
        }
    }
}
