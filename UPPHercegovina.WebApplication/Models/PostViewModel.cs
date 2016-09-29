using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Extensions;

namespace UPPHercegovina.WebApplication.Models
{
    [DataContractAttribute(Name="Post")]
    public class PostCreateViewModel
    {
        [Required]
        [Display(Name = "Naslov")]
        [DataMember(Name="Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Text")]
        [AllowHtml]
        [DataMember(Name = "Text")]        
        public string Text { get; set; }

        [Display(Name = "Slika")]
        [DataMember(Name = "PictureUrl")]
        public string PictureUrl { get; set; }

        [Display(Name = "Preporučeno")]
        [DataMember(Name = "Recommended")]
        public bool Recommended { get; set; }

        [DataMember(Name = "CategoryId")]
        public int CategoryId { get; set; }

        public static Post CreatePost(PostCreateViewModel model, string authorId)
        {
           return new Post(){
                Title = model.Title,
                Text = model.Text,
                PictureUrl = model.PictureUrl,
                Recommended = model.Recommended,
                Status = true,
                PostDate = DateTime.Now,
                Author = authorId,
                CategoryId = model.CategoryId
            };
        }
    }

    [DataContractAttribute(Name = "Post")]
    public class PostListViewModel
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Naslov")]
        [DataMember(Name = "Title")]        
        public string Title { get; set; }

        [DataMember(Name = "Text")]        
        public string Text { get; set; }

        [Display(Name = "Text")]
        public string TextFragment
        {
            get { return Text.Shorten(); }
        }

        [Display(Name = "Autor")]
        [DataMember(Name = "Author")]        
        public string Author { get; set; }

        [Display(Name = "Vrijeme objave")]
        [DataMember(Name = "PostDate")]                
        public DateTime PostDate { get; set; }

        public static string GetAuthorName(string Id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                string userName = context.Users.Where(u => u.Id == Id).FirstOrDefault().GetDisplayName();

                if (!string.IsNullOrWhiteSpace(userName))
                    return userName;
                else
                    return "Error";//tesko da ce ikad do ovog doci
            }
        }
    }
}