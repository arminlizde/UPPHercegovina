using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;


namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name = "Post")]
    public class Post
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Naslov")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        public string Title_short
        {
            get
            {
                return Title.Length > 50 ? Title.Substring(0, 45) + "..." : Title;
            }
        }

        public string Title_link
        {
            get
            {
                return Title.Length > 15 ? Title.Substring(0, 14) + "..." : Title;
            }
        }

        [Required]
        [AllowHtml]
        [Display(Name = "Text")]
        [DataMember(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Autor")]
        [DataMember(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Vrijeme objave")]
        [DataMember(Name = "PostDate")]
        public DateTime PostDate { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        [Display(Name = "Preporučeno")]
        [DataMember(Name = "Recommended")]
        public bool Recommended { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [DataMember(Name = "CategoryId")]
        public int CategoryId { get; set; }

        //used only for Windows phone part
        [DataMember(Name = "Picture")]
        public byte[] Picture { get; set; }

        public string Text500string
        {
            get
            {
                return Text.Length > 500 ? Text.Substring(0, 499) + "..." : Text;
            }
        }

        public MvcHtmlString TextNotIncluded
        {
            get { return MvcHtmlString.Create("which is <strong>not</strong> included in the Quote"); }
        }

        public string AuthorName
        {
            get
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return context.Users.Find(Author).GetDisplayName();
                }
            }
        }

        public List<Post> GetRelatedPosts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Posts.OrderByDescending(p => p.PostDate)
                    .Where(p => p.CategoryId == CategoryId)
                    .Where(p => p.Id != Id)
                    .Take(5)
                    .ToList();
            }
        }

        public List<Post> GetRecommendedPosts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Posts.OrderByDescending(p => p.PostDate)
                    .Where(p => p.Recommended == true)
                    .Where(p => p.Id != Id)
                    .Take(5)
                    .ToList();
            }
        }

        public List<Post> GetLastPosts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Posts.OrderByDescending(p => p.PostDate)
                    .Where(p => p.Id != Id)
                    .Take(5)
                    .ToList();
            }
        }

        public List<Post> RelatedPost { get; set; }

        public List<Post> RecommendedPost { get; set; }

        public List<Post> LastPost { get; set; }

    }
}