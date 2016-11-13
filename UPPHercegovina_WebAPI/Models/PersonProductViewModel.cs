using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class PersonProductViewModel
    {

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

        [JsonProperty("Product")]
        public string Product { get; set; }

        [JsonProperty("Producer")]
        public string Producer { get; set; }

        [JsonProperty("ProducerId")]
        public string ProducerId { get; set; }

        [JsonProperty("Bruto")]
        public string Bruto { get; set; }

        [JsonProperty("Neto")]
        public string Neto { get; set; }

        [JsonProperty("HarvestDate")]
        public DateTime HarvestDate { get; set; }

        [JsonProperty("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("Quality")]
        public string Quality { get; set; }

        [JsonProperty("Warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty("ValuePerKg")]
        public decimal ValuePerKg { get; set; }

        [JsonProperty("ValueSum")]
        public decimal ValueSum { get; set; }

        [JsonProperty("WarehouseId")]
        public int WarehouseId { get; set; }

        [JsonProperty("Damaged")]
        public bool Damaged { get; set; }

        [JsonProperty("Uregntly")]
        public bool Uregntly { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        [JsonProperty("InWarehouse")]
        public bool InWarehouse { get; set; }

        [JsonProperty("Accepted")]
        public bool Accepted { get; set; }
    }
}