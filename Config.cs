using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TooLateToBan
{
    public class PluginConfig : BasePluginConfig
    {
        [JsonPropertyName("MaximumCacheSize")] public int MaximumCacheSize { get; set; } = 10;
        [JsonPropertyName("Debug")] public bool Debug { get; set; } = false;
        [JsonPropertyName("CommandName")] public string CommandName { get; set; } = "tltb";
        [JsonPropertyName("Reasons")] public List<string> Reasons { get; set; } = [
            "Hacking",
            "Voice Abuse",
            "Chat Abuse",
            "Admin disrespect",
            "Other"
        ];
    }
}
