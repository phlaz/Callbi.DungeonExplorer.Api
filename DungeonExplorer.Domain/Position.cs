using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DungeonExplorer.Api.Domain;

public class Position 
{
    [Range(0, 50, ErrorMessage = "X must be between 0 and 50"), JsonPropertyName("x")]
    public int X { get; set; }

    [Range(0, 50, ErrorMessage = "Y must be between 0 and 50"), JsonPropertyName("y")]
    public int Y { get; set; }
}
