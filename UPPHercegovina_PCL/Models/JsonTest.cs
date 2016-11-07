using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class JsonTest
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}