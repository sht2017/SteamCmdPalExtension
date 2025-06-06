using System.Text.Json.Serialization;

namespace SteamCmdPalExtension.Helpers.DebuggerHelper.JsonRPC;

internal sealed class Error<TParams>
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    [JsonPropertyName("message")]
    public required string Message { get; set; }
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TParams? Data { get; set; }
}
