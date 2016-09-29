using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class PictureHelper
    {
        private string _path;
        private HttpPostedFileBase _file;

        public PictureHelper(HttpPostedFileBase file, string path)
        {
            _file = file;
            _path = path;
        }

        public bool Save()
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var ext = Path.GetExtension(_file.FileName);

            if (!allowedExtensions.Contains(ext.ToLower()))
                return false;

            _file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(_path)));

            return true;
        }
    }
}



