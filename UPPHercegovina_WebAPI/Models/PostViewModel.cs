using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_WebAPI.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Title100Char { get { return Title.Length < 100 ? Title : Title.Substring(0, 99); } }

        public string Text { get; set; }
        public string Text500Char { get { return Text.Length < 500 ? Title : Text.Substring(0, 499); } }

        public string AuthorName { get; set; }

        public DateTime PostDate { get; set; }
        public string PostDateOnlyDate { get { return PostDate.ToShortDateString(); } }

        public string PictureUrl { get; set; }

        public bool Recommended { get; set; }

        public byte[] Picture { get; set; }
    }
}