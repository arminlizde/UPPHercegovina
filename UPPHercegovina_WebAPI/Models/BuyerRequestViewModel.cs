using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class BuyerRequestViewModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("BuyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("Details")]
        public string Details { get; set; }

        [JsonProperty("PickUpDate")]
        public DateTime PickUpDate { get; set; }

        [JsonProperty("DateOfCreation")]
        public DateTime DateOfCreation { get; set; }

        [JsonProperty("Accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("PickedUp")]
        public bool PickedUp { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        [JsonProperty("Products")]
        public List<PersonProductViewModel> Products  { get; set; }
    }
}