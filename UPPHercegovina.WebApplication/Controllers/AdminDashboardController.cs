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
        private AdminDashboardViewModel advm = new AdminDashboardViewModel();

        public ActionResult Index()
        {
            //GetNumbersOfUsersByRole();
            PerfChart();
            ProductsInWarehouse();
            ProductMaarks();
            Townships();
            Warehouses();
            BrutoNeto();
            SetGoogleMap();
            return View(advm);
        }

        private void SetGoogleMap()
        {
            var user = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.Location = context.PlaceOfResidences.Find(2);
            ViewBag.Markers = context.Warehouses1.ToList();
        }

        private void BrutoNeto()
        {
            List<PersonProduct> soldProducts = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false).ToList();
            List<PersonProduct> sumProducts = context.PersonProducts.Where(x => x.Accepted == true).ToList();

            foreach (var item in soldProducts)
            {
                advm.BrutoSold += Convert.ToInt32(item.Bruto);
                advm.NetoSold += Convert.ToInt32(item.Neto);

            }

            foreach (var item in sumProducts)
            {
                advm.BrutoSum += Convert.ToInt32(item.Bruto);
                advm.NetoSum += Convert.ToInt32(item.Neto);

            }

        }

        private void Warehouses()
        {
            List<Warehouse1> _warehouses = new List<Warehouse1>();
            _warehouses = context.Warehouses1.ToList();
            GeographicPosition gp = new GeographicPosition();
            WarehouseTownshipViewModel sovm = new WarehouseTownshipViewModel();
            advm.Warehouses = new List<WarehouseTownshipViewModel>();
            foreach (var item in _warehouses)
            {
                gp = item.GeographicPosition;
                sovm.Name = item.Name;
                sovm.Latitude = gp.Latitude;
                sovm.Longitude = gp.Longitude;
                advm.Warehouses.Add(sovm);
            }

            List<Warehouse1> warehouses = context.Warehouses1.ToList();
            List<GeographicPosition> locations = new List<GeographicPosition>();

            foreach (var warehouse in warehouses)
            {
                locations.Add(warehouse.GeographicPosition);
            }

            ViewBag.lokacijeSkladista = locations;

        }

        private void Townships()
        {
            List<Township> townships = new List<Township>();
            townships = context.PlaceOfResidences.ToList();
            GeographicPosition gp = new GeographicPosition();
            WarehouseTownshipViewModel sovm = new WarehouseTownshipViewModel();
            advm.Townships = new List<WarehouseTownshipViewModel>();

            foreach (var item in townships)
            {
                gp = item.GeographicPosition;
                sovm.Latitude = gp.Latitude;
                sovm.Longitude = gp.Longitude;
                sovm.Name = item.Name;
                advm.Townships.Add(sovm);
            }


            List<GeographicPosition> locations = new List<GeographicPosition>();

            foreach (var warehouse in townships)
            {
                locations.Add(warehouse.GeographicPosition);
            }

            ViewBag.lokacijeOpstina = locations;
        }


        //ovo treba pogledati 
        private void ProductMaarks()
        {
            List<PersonProduct> soldProducts = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false).ToList();
            List<Product> products = context.Products.ToList();
            List<PersonProductMark> ppm = context.PersonProductMarks.ToList();
            advm.ProductsMark = new List<ProductMarkViewModel>();

            ProductMarkViewModel productMark = new ProductMarkViewModel();

            for (int i = 0; i < soldProducts.Count(); i++)
            {
                productMark.UserId = soldProducts[i].UserId;
                ApplicationUser user = context.Users.Find(productMark.UserId);
                productMark.ProducerFullName = user.GetFirstLastName();
                productMark.AverageMark = user.GetAverageMark;
                advm.ProductsMark.Add(productMark);

            }

            ArrayList header = new ArrayList { "Proizvodjac ", "Ocjena:" };
            ArrayList data = new ArrayList();
            data.Add(header);
            for (int i = 0; i < advm.ProductsMark.Count(); i++)
            {
                ArrayList data1 = new ArrayList { advm.ProductsMark[i].ProducerFullName, advm.ProductsMark[i].AverageMark };
                data.Add(data1);
            }


            // convert it in json
            string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
            // store it in viewdata/ viewbag
            ViewBag.SMDataa = new HtmlString(dataStr);
            //TempData["data"] = new HtmlString(dataStr);
            //dvm.htmlstringSUPM = ViewBag.Dataa;

        }
        //ovo treba pogledati


        private void ProductsInWarehouse()
        {
            advm.ProductsInWarehouse = null;
            List<PersonProduct> productsInWarehouse = context.PersonProducts.Where(x => x.Accepted == true && x.Status == true).ToList();
            advm.ProductsInWarehouse = productsInWarehouse;
        }

        private void PerfChart()
        {
            List<Product> proizvodi = context.Products.ToList();
            List<PersonProduct> producersProducts = context.PersonProducts.Where(x => x.Accepted == true).ToList();

            advm.NumberOfProductsCount = proizvodi.Count();
            foreach (var item in producersProducts)
            {
                advm.SumKG += Convert.ToInt32(item.Bruto);

            }

            List<PersonProduct> proizvodiKorisnikaProdani = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false).ToList();
            foreach (var item in proizvodiKorisnikaProdani)
            {
                advm.SoldCountKG += Convert.ToInt32(item.Bruto);

            }
        }

        private void GetNumbersOfUsersByRole()
        {
            var roleList = context.Roles.ToList();

            List<ApplicationUser> users = context.Users.Where(x => x.EmailConfirmed == true).ToList();
            var roleKorisnik = roleList.Where(x => x.Name == "Korisnik").FirstOrDefault();

            foreach (var user in users)
            {


            }

        }

    }

}