using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/location")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        
        public LocationController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("name")]
        public async Task<ActionResult> GetLocationName(
            [FromQuery(Name = "latitude")] double latitude,
            [FromQuery(Name = "longitude")] double longitude)
        {
            var client = _httpClientFactory.CreateClient();

            var accessToken = _configuration["MAPBOX:TOKEN"];
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