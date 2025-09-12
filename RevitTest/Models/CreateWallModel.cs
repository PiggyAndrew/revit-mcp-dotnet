using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitTest.Models
{
    public class CreateWallData
    {
        [JsonProperty(PropertyName = "command")]
        public string Command { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "arguments")]
        public CreateWallArguments Args { get; set; }
    }

    public class CreateWallArguments
    {
        [JsonProperty(PropertyName = "start")]
        public double[] Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public double[] End { get; set; }
    }

}
