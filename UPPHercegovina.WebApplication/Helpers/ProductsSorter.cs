using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class ProductsSorter : Sorter
    {
        private string _sortOrder;

        public ProductsSorter(string sortOrder)
        {
            _sortOrder = sortOrder;
        }
      
        public override List<ProductViewModel> Sort(List<ProductViewModel> products)
        {
            switch (_sortOrder)
            {
                case "Name":
                    return products.OrderBy(p => p.Name).ThenBy(p => p.ProductTypeId).ToList();                
                case "name_desc":
                    return products.OrderByDescending(p => p.Name).ThenBy(p => p.ProductTypeId).ToList();                   
                case "Type":
                    return products.OrderBy(p => p.ProductTypeId).ThenBy(p => p.Name).ToList();
                case "type_desc":
                    return products.OrderByDescending(p => p.ProductTypeId).ThenBy(p => p.Name).ToList();
                default:
                    return products.OrderBy(p => p.ProductTypeId).ThenBy(p => p.Name).ToList();
            }
        }

    }
}