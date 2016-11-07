using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_PCL.Models
{
    public class PostViewModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }
        public string Title100Char { get { return Title.Length < 100 ? Title : Title.Substring(0, 99); } }

        [JsonProperty("Text")]
        public string Text { get; set; }
        public string Text500Char { get { return Text.Length < 500 ? Title : Text.Substring(0, 499); } }

        [JsonProperty("AuthorName")]
        public string AuthorName { get; set; }

        [JsonProperty("PostDate")]
        public DateTime PostDate { get; set; }
        public string PostDateOnlyDate { get { return PostDate.Date.ToString(); } }

        [JsonProperty("PictureUrl")]
        public string PictureUrl { get; set; }

        [JsonProperty("Recommended")]
        public bool Recommended { get; set; }

        [JsonProperty("Picture")]
        public byte[] Picture { get; set; }


    }
}