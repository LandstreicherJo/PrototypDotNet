using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.JSInterop;
using PrototypDotNet.Dtos;
using PrototypDotNet.Entities;
using System;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PrototypDotNet.Services
{
    public class MeasuredDataService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public MeasuredDataService(IConfiguration config)
        {
            _config = config;

            string uri = _config.GetValue<string>("SUPABASE_URL");
            string key = _config.GetValue<string>("SUPABASE_KEY");

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(uri);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + key);
            _httpClient.DefaultRequestHeaders.Add("apikey", key);
        }

        public async Task<HttpResponseMessage> CreateEntryAsync(MeasuredDataEntryDto e)
        {
            return await _httpClient.PostAsJsonAsync("MeasuredDataEntries", e);
        }

        public async Task<IEnumerable<MeasuredDataEntry>?> ReadEntriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MeasuredDataEntry>>("MeasuredDataEntries");
        }

        public async Task<MeasuredDataEntry?> FindByIdAsync(Guid guid)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<MeasuredDataEntry>>("MeasuredDataEntries?id=eq." + guid + "&select=*");
            return (response == null) ? null : response.First();
        }

        public async Task<HttpResponseMessage> UpdateEntryAsync(MeasuredDataEntry e)
        {
            return await _httpClient.PutAsJsonAsync("MeasuredDataEntries?id=eq." + e.Id, e);
        }

        public async Task<HttpResponseMessage> DeleteEntryAsync(Guid guid)
        {
            return await _httpClient.DeleteAsync("MeasuredDataEntries?id=eq." + guid);
        }



        public async Task<IEnumerable<MeasuredDataEntry>?> ReadByTimespan(string timespan)
        {
            string start = timespan.Split(";")[0];
            if (start.Contains(" ")) start = start.Split(" ")[0];
            string end = timespan.Split(";")[1];
            if (end.Contains(" ")) end = end.Split(" ")[0];
            return await _httpClient.GetFromJsonAsync<IEnumerable<MeasuredDataEntry>?>("MeasuredDataEntries?created=gt." + start + "&created=lt." + end + "&select=*");
        }

        public double CalculateAverage(IEnumerable<MeasuredDataEntry> matchedEntries)
        {
            double sum = 0;
            foreach (var entry in matchedEntries)
            {
                if(entry.Temperature.HasValue) sum += entry.Temperature.Value;
            }
            return sum / matchedEntries.Count();
        }
    }
}
