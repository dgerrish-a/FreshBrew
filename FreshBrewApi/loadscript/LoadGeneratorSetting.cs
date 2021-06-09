using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadScript
{
    public struct UrlPath
    {
        [JsonProperty("httpVerb")]

        public string HttpVerb { get; set; }

        [JsonProperty("url")]
        public string ApplicationUrl { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; } 
    }
    public class LoadGeneratorSetting
    {
        [JsonProperty("loop")]
        public int Loop { get; set; }
        [JsonProperty("loopwait")]
        public int LoopWait { get; set; }
        [JsonProperty("threads")]
        public int Threads { get; set; }
        [JsonProperty("hits")]
        public UrlPath[]  urlPathList { get; set; }

    }
}
