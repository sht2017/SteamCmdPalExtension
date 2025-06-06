using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SteamCmdPalExtension.Helpers.WebSocketHelper;

// See https://chromedevtools.github.io/devtools-protocol/#get-jsonversion

[JsonSourceGenerationOptions(UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip)]
[JsonSerializable(typeof(Version))]
internal sealed partial class VersionContext : JsonSerializerContext { }

internal sealed class Version
{
    [JsonPropertyName("Browser")]
    public required string Browser { get; set; }

    [JsonPropertyName("Protocol-Version")]
    public required string ProtocolVersion { get; set; }

    [JsonPropertyName("User-Agent")]
    public required string UserAgent { get; set; }

    [JsonPropertyName("V8-Version")]
    public required string V8Version { get; set; }

    [JsonPropertyName("WebKit-Version")]
    public required string WebkitVersion { get; set; }

    [JsonPropertyName("webSocketDebuggerUrl")]
    public required string WebSocketDebuggerUrl { get; set; }

    internal static async Task<Version> GetVersion(Uri targetUrl)
    {
        using HttpClient client = new();
        string json = await client.GetStringAsync(new Uri(targetUrl, "json/version")).ConfigureAwait(false);

        Version? version = JsonSerializer.Deserialize(json, VersionContext.Default.Version);

        return version ?? throw new JsonException("Cannot deserialize version json. Result was null.");
    }
}