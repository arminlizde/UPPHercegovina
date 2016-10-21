using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    //singleton pattern
    public class GlobalData
    {
        private static GlobalData _instance;
        private static object _syncRoot = new object();

        public List<PersonProduct> forDelivery;

        public List<PersonProduct> ReservedProducts;

        public string CurrentUserId;

        private GlobalData()
        {
            forDelivery = new List<PersonProduct>();
            ReservedProducts = new List<PersonProduct>();
        }

        public static GlobalData Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new GlobalData();
                    }
                    return _instance;

                }
            }
        }
    }
}