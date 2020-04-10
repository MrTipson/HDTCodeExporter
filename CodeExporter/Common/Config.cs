using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Config
    {
        [JsonProperty(PropertyName = "FilePath")]
        public string FilePath { get; set; }
    }
}
