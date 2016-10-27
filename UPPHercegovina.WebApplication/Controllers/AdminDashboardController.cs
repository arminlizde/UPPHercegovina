using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;
namespace UPPHercegovina.WebApplication.Models
{
    public class AdminDashboardController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ADVM advm = new ADVM();
        // GET: AdminDashboard
        public ActionResult Index()
        {
            //GetBrojeve();
            PerfChart();
            ProizvodiUSkladistu();
            OcjeneProizvoda();
            Opstine();
            Skladista();
            BrutoNeto();
            SetGoogleMap();
            return View(advm);
        }

        private void SetGoogleMap()
        {
            var user = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            //This works only if we allow only registered users to visit 
            ViewBag.Location = context.PlaceOfResidences.Find(2);
            ViewBag.Markers = context.Warehouses1.ToList();

           
        }

        private void BrutoNeto()
        {
            List<PersonProduct> _prodaniProizvodi = context.PersonProducts.Where(x => x.Accepted == true && x.Status==false).ToList();
            List<PersonProduct> _ukupniProizvodi = context.PersonProducts.Where(x => x.Accepted == true).ToList();

            foreach (var item in _prodaniProizvodi)
            {
                advm.brutoProdani +=Convert.ToInt32( item.Bruto);
                advm.netoPRodani += Convert.ToInt32(item.Neto);

            }

            foreach (var item in _ukupniProizvodi)
            {
                advm.brutoUkupni += Convert.ToInt32(item.Bruto);
                advm.netoUkupni += Convert.ToInt32(item.Neto);

            }

        }

        private void Skladista()
        {
            List<Warehouse1> skladista = new List<Warehouse1>();
            skladista = context.Warehouses1.ToList();
            GeographicPosition gp = new GeographicPosition();
            SkladistaOpstineVM sovm = new SkladistaOpstineVM();
            advm.skladista = new List<SkladistaOpstineVM>();
            foreach (var item in skladista)
            {
                gp = item.GeographicPosition;
                sovm.naziv = item.Name;
                sovm.lat = gp.Latitude;
                sovm.lng = gp.Longitude;
                advm.skladista.Add(sovm);
            }


            //ArrayList header = new ArrayList { "Lat", "Name", "Long" };
            //ArrayList data = new ArrayList();
            //data.Add(header);
            //for (int i = 0; i < advm.skladista.Count(); i++)
            //{
            //    ArrayList data1 = new ArrayList { advm.skladista[i].lng.ToString(), advm.skladista[i].naziv, advm.skladista[i].lat };
            //    data.Add(data1);
            //}


            // convert it in json
           // string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
            // store it in viewdata/ viewbag
            //ViewBag.SMDataa = new HtmlString(dataStr);


            List<Warehouse1> warehouses = context.Warehouses1.ToList();
            List<GeographicPosition> locations = new List<GeographicPosition>();

            foreach (var warehouse in warehouses)
            {
                locations.Add(warehouse.GeographicPosition);
            }

            ViewBag.lokacijeSkladista = locations;

        }

        private void Opstine()
        {
            List<Township> opstine = new List<Township>();
            opstine = context.PlaceOfResidences.ToList();
            GeographicPosition gp = new GeographicPosition();
            SkladistaOpstineVM sovm = new SkladistaOpstineVM();
            advm.opstine = new List<SkladistaOpstineVM>();

            foreach (var item in opstine)
            {
                gp = item.GeographicPosition;
                sovm.lat = gp.Latitude;
                sovm.lng = gp.Longitude;
                sovm.naziv = item.Name;
                advm.opstine.Add(sovm);
            }


            List<GeographicPosition> locations = new List<GeographicPosition>();

            foreach (var warehouse in opstine)
            {
                locations.Add(warehouse.GeographicPosition);
            }

            ViewBag.lokacijeOpstina = locations;
        }

        private void OcjeneProizvoda()
        {
            List<PersonProduct> _prodaniProizvodi = context.PersonProducts.Where(x => x.Accepted == true && x.Status==false).ToList();
            List<Product> proizvodi = context.Products.ToList();
            List<PersonProductMark> ppm = context.PersonProductMarks.ToList();
            advm.ProizvodiOcjene = new List<ProizvodOcjenaVM>();

            //int brojOcjena = 0;
            //string userName;
            //int userID;
            //double ocjene = 0;

            ProizvodOcjenaVM po = new ProizvodOcjenaVM();

            for (int i = 0; i < _prodaniProizvodi.Count(); i++)
            {
                po.userID = _prodaniProizvodi[i].UserId;
                ApplicationUser user = context.Users.Find(po.userID);
                po.ImePrezimeProizvodjaca = user.GetFirstLastName();
                po.ProsjecnaOcjena = user.GetAverageMark;
                advm.ProizvodiOcjene.Add(po);

            }

            //advm.ProizvodiOcjene.Select(x => x.ImePrezimeProizvodjaca).Distinct();

            ArrayList header = new ArrayList { "Proizvodjac ", "Ocjena:" };
            ArrayList data = new ArrayList();
            data.Add(header);
            for (int i = 0; i < advm.ProizvodiOcjene.Count(); i++)
            {
                ArrayList data1 = new ArrayList { advm.ProizvodiOcjene[i].ImePrezimeProizvodjaca, advm.ProizvodiOcjene[i].ProsjecnaOcjena };
                data.Add(data1);
            }


            // convert it in json
            string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
            // store it in viewdata/ viewbag
            ViewBag.SMDataa = new HtmlString(dataStr);
            //TempData["data"] = new HtmlString(dataStr);
            //dvm.htmlstringSUPM = ViewBag.Dataa;

        }

        private void ProizvodiUSkladistu()
        {
            advm.proizvodiUskladistu = null;
            List<PersonProduct> _proizvodiUskladistu = context.PersonProducts.Where(x => x.Accepted == true && x.Status==true).ToList();
            advm.proizvodiUskladistu = _proizvodiUskladistu;
        }

        private void PerfChart()
        {
            List<Product> proizvodi = context.Products.ToList();
            List<PersonProduct> proizvodiKorisnika = context.PersonProducts.Where(x=>x.Accepted==true).ToList();

            advm.brojProizvodaUkupno = proizvodi.Count();
            foreach (var item in proizvodiKorisnika)
            {
                advm.brojUkupnoKG += Convert.ToInt32(item.Bruto);

            }


            List<PersonProduct> proizvodiKorisnikaProdani = context.PersonProducts.Where(x => x.Accepted == true && x.Status==false).ToList();
            foreach (var item in proizvodiKorisnikaProdani)
            {
                advm.brojProdanihKG += Convert.ToInt32(item.Bruto);

            }

        }

        private void GetBrojeve()
        {
            var roleList = context.Roles.ToList();

            List<ApplicationUser> users = context.Users.Where(x => x.EmailConfirmed == true).ToList();
            var roleKorisnik = roleList.Where(x => x.Name == "Korisnik").FirstOrDefault();

            foreach (var user in users)
            {
                
               
            }

        }

        // GET: AdminDashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminDashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminDashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminDashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminDashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminDashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
