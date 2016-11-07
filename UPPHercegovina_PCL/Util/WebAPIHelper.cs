using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UPPHercegovina_PCL.Util
{
   public class WebAPIHelper
    {
        private HttpClient _client { get; set; }

        private string _route { get; set; }

        public WebAPIHelper(string uri, string route)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(uri);
            this._route = route;

        }

        public HttpResponseMessage GetResponse()
        {
            return _client.GetAsync(_route).Result;
        }

        //sta znaci U
        public HttpResponseMessage GetResponse(string u)
        {
            return _client.GetAsync(_route + "/" + u).Result;
        }

        public HttpResponseMessage GetActionResponse(string action, string parametar = "")
        {
            return _client.GetAsync(_route + "/" + action + "/" + parametar).Result;
        }

        public HttpResponseMessage GetActionResponse(string action, string parametar = "", string parametar2 = "")
        {
            return _client.GetAsync(_route + "/" + action + "/" + parametar + "/" + parametar2).Result;
        }

        public HttpResponseMessage GetActionResponse(string action, string param1 = "", string param2 = "", string param3 = "",
           string param4 = "", string param5 = "", string param6 = "")
        {
            return _client.GetAsync(_route + "/" + action + "/" + param1 + "/" + param2 + "/" + param3
                + "/" + param4 + "/" + param5 + "/" + param6).Result;
        }

        public HttpResponseMessage GetActionResponse(string action, Object parametar = null)
        {
            return _client.GetAsync(_route + "/" + action + "/" + parametar).Result;
        }


        public HttpResponseMessage PostResponse(Object newObject)
        {
            return _client.PostAsJsonAsync(_route, newObject).Result;
        }

        public HttpResponseMessage PutResponse(int id, Object nonew)
        {
            return _client.PutAsJsonAsync(_route + "/" + id, nonew).Result;
        }

        public HttpResponseMessage DeleteActionResponse(string id)
        {
            return _client.DeleteAsync(_route + "/" + id).Result;
        }
    }
}
