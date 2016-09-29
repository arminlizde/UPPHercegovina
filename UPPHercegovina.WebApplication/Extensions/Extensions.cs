using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace UPPHercegovina.WebApplication.Extensions
{
    public static class Extensions
    {
        //ovo uopste nije extension ...
        public static string GetRndNumber()
        {
            Random rnd = new Random();
            int number = rnd.Next(0, 10000);

            return number.ToString();
        }

        public static string Shorten(this string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                return string.Empty;

            string shortText = tekst;

            if (tekst.Length > 15)
                shortText = tekst.Substring(0, 15);

            return shortText + " ...";
        }

    }
}