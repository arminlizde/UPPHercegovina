using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UPPHercegovina_PCL.Util
{
    public class ClientHelper
    {
        public HttpClient _client;
        private Uri _url;
        public FormatRequest formatRequest;

        internal ClientHelper(Uri url)
        {
            _client = new HttpClient();
            _url = url;
            formatRequest = new FormatRequest();
        }

        //        internal void ChangeUrl(Uri url)
        //        {
        //            _url = url;
        //        }

        //        internal async Task<string> Post()
        //        {
        //            try
        //            {
        //                var response = await _client.PostAsync(_url, new HttpStringContent(formatRequest.toXml()));
        //                return await response.Content.ReadAsStringAsync();
        //            }
        //            catch (Exception e) { return e.Message; }
        //        }

        //        internal async Task<string> Put()
        //        {
        //            try
        //            {
        //                var response = await _client.PutAsync(_url, new HttpStringContent(formatRequest.toXml()));
        //                return await response.Content.ReadAsStringAsync();
        //            }
        //            catch (Exception e)
        //            {
        //                var st = e.Message;
        //            }

        //            return null;
        //        }

        //        internal async Task<string> Delete()
        //        {
        //            try
        //            {
        //                var response = await _client.DeleteAsync(_url);
        //                return await response.Content.ReadAsStringAsync();
        //            }
        //            catch (Exception e)
        //            {
        //                return e.Message;
        //            }
        //        }

        //internal async Task<string> Get()
        //{
        //    try
        //    {
        //        var response = await _client.GetAsync(_url).AsTask();
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        return e.Message;
        //    }
        //}

        //        internal async Task<string> Get3()
        //        {
        //            var response = await _client.GetAsync(_url).AsTask();
        //            var buffer = await response.Content.ReadAsBufferAsync();
        //            byte[] rawBytes = new byte[buffer.Length];
        //            using (var reader = DataReader.FromBuffer(buffer))
        //            {
        //                reader.ReadBytes(rawBytes);
        //            }

        //            var res = Encoding.UTF8.GetString(rawBytes, 0, rawBytes.Length);

        //            return res;
        //        }

        //        /// <summary>
        //        /// 
        //        /// Ovaj get poziv služi za dobijanje naših karaktera čćšđž
        //        /// </summary>
        //        /// <returns></returns>
        //        internal async Task<string> Get2()
        //        {
        //            var filter = new HttpBaseProtocolFilter();
        //            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
        //            HttpClient client = new HttpClient(filter);
        //            HttpWebRequest webrequest =
        //                   (HttpWebRequest)WebRequest.Create(_url);
        //            webrequest.Method = "GET";
        //            WebResponse response = await webrequest.GetResponseAsync();

        //            Stream responseStream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(responseStream);

        //            return reader.ReadToEnd();
        //        }

    }
}
