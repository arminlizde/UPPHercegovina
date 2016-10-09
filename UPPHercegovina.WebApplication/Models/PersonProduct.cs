using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UPPHercegovina.WebApplication.Models
{
    public class PersonProduct
    {
        [DataMember(Name = "Id")] 
        public int Id { get; set; }

        [Display(Name = "Korisnik")]
        [DataMember(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Proizvod")]
        [DataMember(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Datum Berbe")]
        [DataMember(Name = "HarvestDate")]
        public DateTime HarvestDate { get; set; }

        public string HarvestOnlyDate
        {
            get
            {
                return HarvestDate.Date.ToShortDateString();
            }
        }

        [Display(Name = "Neto količina")]
        [DataMember(Name = "Neto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Samo decimalni brojevi")]
        public string Neto { get; set; }

        [Display(Name = "Bruto količina")]
        [DataMember(Name = "Bruto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Samo decimalni brojevi")]
        public string Bruto { get; set; }

        [Display(Name = "Rok trajanja")]
        [DataMember(Name = "ExparationDate")]
        public DateTime ExparationDate { get; set; }

        public string ExparationOnlyDate
        {
            get
            {
                return ExparationDate.Date.ToShortDateString();
            }
        }

        [Display(Name = "Kvalitet")]
        [DataMember(Name = "Quality")]
        public string Quality { get; set; }

        [Display(Name = "Skladište")]
        [DataMember(Name = "Warehouse1Id")]
        public int? Warehouse1Id { get; set; }

        [Display(Name = "Zemljište")]
        [DataMember(Name = "FieldId")]
        public int FieldId { get; set; }

        [Display(Name = "Oštećeno")]
        [DataMember(Name = "Damaged")]
        public bool Damaged { get; set; }

        [Display(Name = "Očekivana vrijednost")]
        [DataMember(Name = "CircaValue")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal CircaValue { get; set; }

        [Display(Name = "Hitno")]
        [DataMember(Name = "Urgently")]
        public bool Urgently { get; set; }

        [Display(Name = "Prihvaćeno")]
        [DataMember(Name = "Accepted")]
        public bool Accepted { get; set; }

        [Display(Name = "Status")]
        [DataMember(Name = "Status")]
        public bool Status { get; set; }

        [DataMember(Name = "Product")]
        public virtual Product Product { get; set; }

        [DataMember(Name ="Field")]
        public virtual Field Field { get; set; }

        [DataMember(Name = "ApplicationUser")]
        public virtual ApplicationUser User { get; set; }

        [DataMember(Name = "Warehouse1")]
        public virtual Warehouse1 Warehouse1 { get; set; }
    }

    public class PersonProductIndexViewModel
    {
        [Required]
        [Display(Name = "Proizvod")]
        [DataMember(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Datum Berbe")]
        [DataMember(Name = "HarvestDate")]
        public DateTime HarvestDate { get; set; }



        [Display(Name = "Neto količina")]
        [DataMember(Name = "Neto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Samo decimalni brojevi")]
        public string Neto { get; set; }

        [Display(Name = "Bruto količina")]
        [DataMember(Name = "Bruto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Samo decimalni brojevi")]
        public string Bruto { get; set; }

        [Display(Name = "Rok trajanja")]
        [DataMember(Name = "ExparationDate")]
        public DateTime ExparationDate { get; set; }

        [Display(Name = "Zemljište")]
        [DataMember(Name = "FieldId")]
        public int FieldId { get; set; }

        [Display(Name = "Oštećeno")]
        [DataMember(Name = "Damaged")]
        public bool Damaged { get; set; }

        [Display(Name = "Očekivana vrijednost")]
        [DataMember(Name = "CircaValue")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal CircaValue { get; set; }

        [Display(Name = "Hitno")]
        [DataMember(Name = "Urgently")]
        public bool Urgently { get; set; }

        [DataMember(Name = "Product")]
        public Product Product { get; set; }

        [DataMember(Name = "Field")]
        public Field Field { get; set; }
    }
}