using System.Text.Json.Serialization;

namespace ChatService.Models;

internal class PaginatedResults<T>
    where T : class
{
    public PaginatedResults(int totalResults, IReadOnlyCollection<T> results)
    {
        TotalResults = totalResults;
        Results = results;
    }

    [JsonPropertyName("totalNumberOfResults")]
    public int TotalResults { get; }
    
    public IReadOnlyCollection<T> Results { get; }
}