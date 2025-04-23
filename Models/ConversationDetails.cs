using System.Text.Json.Serialization;

namespace IntercomCsvConverter.Models;

public class ConversationDetails
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("source")]
    public Question Question { get; set; }
}