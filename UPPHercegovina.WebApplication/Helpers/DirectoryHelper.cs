using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class DirectoryHelper
    {
        private string _path;

        public DirectoryHelper(string path)
        {
            _path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(path));
        }

        public bool Create()
        {
                if (Directory.Exists(_path))
                    return false;
                    
                    //ona ce napraviti samo ako ne postoji, ne treba mi gore ona provjera
                DirectoryInfo di = Directory.CreateDirectory(_path);

            return true;
        }

        public bool Delete()
        {
            if (!Directory.Exists(_path))
                return false;

            DirectoryInfo di = new DirectoryInfo(_path);
            di.Delete();

            return true;
        }
    }
}