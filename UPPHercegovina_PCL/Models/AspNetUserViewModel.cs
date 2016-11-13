﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina_PCL.Models
{
    public class AspNetUserViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty("SecurityStamp")]
        public string SecurityStamp { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("IdentificationNumber")]
        public string IdentificationNumber { get; set; }

        [JsonProperty("PictureUrl")]
        public string PictureUrl { get; set; }

        [JsonProperty("Township")]
        public string Township { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("Error")]
        public bool Error { get; set; }

        [JsonProperty("Role")]
        public string Role { get; set; }

        public string FullName { get { return String.Format("{0} {1}", FirstName, LastName); } }



        public string AverageMark { get; set; }
        public int BrutoAddedKg { get; set; }
        public int BrutoSoldKg { get; set; }
        public int NetoAddedKg { get; set; }
        public int NetoSoldKg { get; set; }
        public int BrutoDiffKg { get; set; }
        public int NetoDiffKg { get; set; }
    }
}