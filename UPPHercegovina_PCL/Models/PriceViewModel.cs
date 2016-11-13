using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_PCL.Models
{
    public class PriceViewModel
    {
        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("MinValue")]
        public string MinValue { get; set; }

        public decimal MinValueDec { get { return Convert.ToDecimal(MinValue); } }

        [JsonProperty("MaxValue")]
        public string MaxValue { get; set; }

        public decimal MaxValueDec { get { return Convert.ToDecimal(MaxValue); } }

        [JsonProperty("AverageValue")]
        public string AverageValue { get; set; }
    }
}