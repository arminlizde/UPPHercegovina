using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPHercegovina.WebApplication.Models
{
    public class ADVM
    {
        public int brojProizvodjaca { get; set; }
        public int brojNakupaca{ get; set; }
        public int brojAdmina{ get; set; }

        public int brojProizvodaUkupno { get; set; }
        public int brojProdanihKG { get; set; }
        public int brojUkupnoKG { get; set; }
        public List<PersonProduct> proizvodiUskladistu { get; set; }
        public List<ProizvodOcjenaVM> ProizvodiOcjene { get; set; }

        public int brutoUkupni { get; set; }
        public int brutoProdani { get; set; }
        public int netoUkupni { get; set; }
        public int netoPRodani { get; set; }

        public List<SkladistaOpstineVM> skladista { get; set; }
        public List<SkladistaOpstineVM> opstine { get; set; }




    }
}