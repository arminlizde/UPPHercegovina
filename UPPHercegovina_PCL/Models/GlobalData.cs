using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPPHercegovina_PCL.Models
{
    public class GlobalData
    {
        private static GlobalData _instance;
        private static object _syncRoot = new object();

        public string PictureUrlStart { get; set; }
        public string PictureUrlStart2 { get; set; }

        public AspNetUserViewModel CurrentUser { get; set; }

        public string BaseUrl;

        private GlobalData()
        {
            PictureUrlStart = "C:\\Users\\armin_000\\UPPHercegovina.Diplomski\\UPPHercegovina.WebApplication\\";
            PictureUrlStart2 = "UPPHercegovina.WebApplication\\";

            BaseUrl = "http://localhost:57397";
            CurrentUser = new AspNetUserViewModel();
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
