using System.Text.Json.Serialization;

namespace IntercomCsvConverter.Models;

public class Question
{
    [JsonPropertyName("body")]
    public string Body { get; set; }
    
    [JsonPropertyName("delivered_as")]
    public string DeliveredAs { get; set; }
}