using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class AdminDashboardViewModel
    {
        public int ProducerCount { get; set; }
        public int BuyersCount { get; set; }
        public int AdminCount { get; set; }

        public int NumberOfProductsCount { get; set; }
        public int SoldCountKG { get; set; }
        public int SumKG { get; set; }
        public List<PersonProduct> ProductsInWarehouse { get; set; }
        public List<ProductMarkViewModel> ProductsMark { get; set; }

        public int BrutoSum { get; set; }
        public int BrutoSold { get; set; }
        public int NetoSum { get; set; }
        public int NetoSold { get; set; }

        public List<WarehouseTownshipViewModel> Warehouses { get; set; }
        public List<WarehouseTownshipViewModel> Townships { get; set; }
        public List<ProductExpieryByYearViewModel> productExpeieryByyear { get; set; }
    }

    public class WarehouseTownshipViewModel
    {
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public class ProductMarkViewModel
    {
        public string ProducerFullName { get; set; }
        public string AverageMark { get; set; }
        public string UserId { get; set; }
    }

    public class ProductExpieryByYearViewModel
    {
        public string Year { get; set; }
        public string Sales { get; set; }
    }
}