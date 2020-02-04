using GaloreAPIDemoV2.Converters;
using GaloreAPIDemoV2.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;

namespace GaloreAPIDemoV2
{
    public class GaloreClient
    {
        readonly GaloreConnector _connector;
        readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        readonly MediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter();

        public GaloreClient(GaloreConnector connector)
        {
            _connector = connector;
            _jsonOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            _jsonOptions.Converters.Add(new DataPointOrErrorConverter());
        }

        public  async Task<AssetNodeModel> GetAssetById(string id, int maxLevelOfEdges = 0)
        {
            var parameters = new Dictionary<string, string>()
            {
                {  nameof(maxLevelOfEdges), maxLevelOfEdges.ToString() }
            };
            var url = QueryHelpers.AddQueryString($"AssetModel/nodes/{id}", parameters);

            var httpClient = await _connector.GetClient();
            var response = await httpClient.GetAsync(url);
            var responseContentString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<AssetNodeModel>(responseContentString, _jsonOptions);
        }

        public async Task<AssetNodeModel> GetAssetByPath(string path, int maxLevelOfEdges = 0)
        {
            var nodes = await GetAssets(path, null, maxLevelOfEdges);
            return nodes.FirstOrDefault();
        }

        public async Task<List<AssetNodeModel>> GetAssetsByNodeType(string nodeType, int maxLevelOfEdges = 0)
        {
            return await GetAssets(null, nodeType, maxLevelOfEdges);
        }

        public async Task<List<AssetNodeModel>> GetAssets(string? path, string? nodeType, int maxLevelOfEdges = 0)
        {
            var parameters = new Dictionary<string, string>()
            {
                { nameof(maxLevelOfEdges), maxLevelOfEdges.ToString() }
            };
            if (!string.IsNullOrEmpty(path)) {
                parameters.Add(nameof(path), path);
            }
            if (!string.IsNullOrEmpty(nodeType))
            {
                parameters.Add(nameof(nodeType), nodeType);
            }
            var url = QueryHelpers.AddQueryString($"AssetModel/nodes", parameters);

            var httpClient = await _connector.GetClient();
            var response = await httpClient.GetAsync(url);
            var responseContentString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<List<AssetNodeModel>>(responseContentString, _jsonOptions);
        }

        public async Task<TimeseriesResponse> GetTimeseriesData(string timeseriesId, DateTime from, DateTime to, int pageSize = 1000)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromOptions", "included" },
                { "toOptions", "included" },
                { nameof(from), from.ToString("o") },
                { nameof(to), to.ToString("o") },
                { nameof(pageSize), pageSize.ToString() }
            };
            var url = QueryHelpers.AddQueryString($"timeseries/{timeseriesId}", parameters);

            var httpClient = await _connector.GetClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TimeseriesResponse>(content, _jsonOptions);
        }

        public async Task<Dictionary<string, DataPointOrError>> GetLatestTimeseriesForIds(params string[] ids)
        {
            var url = "timeseries/LatestValue";
            var httpClient = await _connector.GetClient();
            var response = await httpClient.PostAsync(url, ids, _jsonFormatter);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<Dictionary<string, DataPointOrError>>(await response.Content.ReadAsStreamAsync(), _jsonOptions);
        }

        public async Task<DataPoint> GetLatestTimeseries(string id)
        {
            var url = $"timeseries/LatestValue/{id}";
            var httpClient = await _connector.GetClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<DataPoint>(await response.Content.ReadAsStreamAsync(), _jsonOptions);
        }
    }
}
