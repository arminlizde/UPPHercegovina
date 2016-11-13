using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class BuyerViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("RegistrationDate")]
        public string RegistrationDate { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Township")]
        public string Township { get; set; }

        [JsonProperty("Role")]
        public string Role { get; set; }

        [JsonProperty("NumberOfSuccededRequests")]
        public int NumberOfSuccededRequests { get; set; }

        [JsonProperty("NumberOfFailedRequests")]
        public int NumberOfFailedRequests { get; set; }

        [JsonProperty("NumberOfBoughtProducts")]
        public int NumberOfBoughtProducts { get; set; }

        [JsonProperty("AverageMark")]
        string AverageMark { get; set; }

        [JsonProperty("Error")]
        public bool Error { get; set; }

    }
}