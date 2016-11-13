using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_PCL.Models
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

        public string PickUpOnlyDate { get { return String.Format("{0}/{1}/{2}", PickUpDate.Day, PickUpDate.Month, PickUpDate.Year); } }

        [JsonProperty("DateOfCreation")]
        public DateTime DateOfCreation { get; set; }

        [JsonProperty("Accepted")]
        public bool Accepted { get; set; }

        public string AcceptedString { get { return Accepted ? "DA" : "NE"; } }

        [JsonProperty("PickedUp")]
        public bool PickedUp { get; set; }

        public string  PickedUpString { get { return PickedUp ? "DA" : "NE"; } }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        public string StatusString { get { return Status ? "DA" : "NE"; } }

        [JsonProperty("Products")]
        public List<PersonProductViewModel> Products  { get; set; }
    }
}