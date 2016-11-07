using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UPPHercegovina_PCL.Util
{
    public class FormatRequest : Dictionary<string, string>
    {
        public string toXml()
        {
            XDocument xdoc = new XDocument();
            XElement response = new XElement("request");
            XElement element;

            foreach (string k in this.Keys)
            {
                element = new XElement(k, this[k]);
                response.Add(element);
            }
            xdoc.Add(response);
            return xdoc.ToString();
        }

        public string toQueryString()
        {
            string query = "";
            foreach (string k in this.Keys)
            {
                query += "/" + k + "/" + this[k];
            }
            return query;
        }
    }
}
