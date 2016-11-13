using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class AppointmentPostReturnModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }

    }
}