using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("location")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public LocationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpGet("name")]
        public async Task<ActionResult> GetLocationName(
            [FromQuery(Name = "latitude")] double latitude,
            [FromQuery(Name = "longitude")] double longitude)
        {
            var client = _httpClientFactory.CreateClient();

            var accessToken = "pk.eyJ1IjoiZGFubnloZWFyZDciLCJhIjoiY2tnZjk2N213MHZnajJ2cXoycTh2anN4bCJ9.rwgWg42fnpap7SfVUEl-Tg";
            var uri =
                $"https://api.mapbox.com/geocoding/v5/mapbox.places/{longitude}%2C{latitude}.json?access_token={accessToken}";
                
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<MapboxResponse>(content);
            var placeName = deserializedResponse.Features.FirstOrDefault(
                feature => feature.PlaceType.Contains("place"));
            if (placeName == null) return NotFound();

            return Ok(new
            {
                Name = placeName.Text
            });
        }

        private class MapboxResponse
        {
            [JsonPropertyName("features")]
            public ICollection<MapboxFeature> Features { get; set; }
        }

        private class MapboxFeature
        {
            [JsonPropertyName("place_type")]
            public ICollection<string> PlaceType { get; set; }
            [JsonPropertyName("relevance")]
            public int Relevance { get; set; }
            [JsonPropertyName("text")]
            public string Text { get; set; }
        }
    }
}