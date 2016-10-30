using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
            GetNumbersOfUsersByRole();
            PerfChart();
            ProductsInWarehouse();
            ProductMaarks();
            Townships();
            Warehouses();
            BrutoNeto();
            YearlyProductExpiery();
            SetGoogleMap();
            return View(advm);
        }

        private void GetNumbersOfUsersByRole()
        {
            var roleList = context.Roles.ToList();
            List<ApplicationUser> users = context.Users.Where(x => x.EmailConfirmed == true && x.Roles.Count() != 0).ToList();
            IdentityRole myRole = context.Roles.Include(r => r.Users).First(r => r.Name == "Korisnik");
            IdentityRole myRole2 = context.Roles.Include(r => r.Users).First(r => r.Name == "Administrator");
            IdentityRole myRole3 = context.Roles.Include(r => r.Users).First(r => r.Name == "Nakupac");
            ViewBag.UserNo = myRole.Users.Count();
            ViewBag.AdminNo = myRole2.Users.Count();
            ViewBag.NNo = myRole3.Users.Count();
        }

        private void YearlyProductExpiery()
        {
            List<ProductExpieryByYearViewModel> productsExpireByYear = new List<ProductExpieryByYearViewModel>();
            List<string> years = new List<string>();
            List<string> years2 = new List<string>();

            List<PersonProduct> products = context.PersonProducts.Where(x => x.Accepted == true && x.Status == true).ToList();
            foreach (var item in products)
            {
                years.Add(item.ExparationDate.Year.ToString());
            }
            years2 = years.Distinct().ToList();
            for (int i = 0; i < years2.Count(); i++)
            {
                ProductExpieryByYearViewModel s = new ProductExpieryByYearViewModel();
                s.Year = years2[i];
                productsExpireByYear.Add(s);
            }
            foreach (var item in productsExpireByYear)
            {
                decimal value = 0;
                List<PersonProduct> pp = products.Where(x => x.ExparationDate.Year == Convert.ToInt32(item.Year)).ToList();
                foreach (var x in pp)
                {
                    value += x.Value;
                }
                item.Sales = value.ToString();
            }

            advm.productExpeieryByyear = productsExpireByYear.OrderByDescending(x => x.Year).ToList();
            ArrayList header = new ArrayList { "Godina ", "Suma vrijednosti:" };
            ArrayList data = new ArrayList();
            data.Add(header);
            for (int i = 0; i < advm.productExpeieryByyear.Count(); i++)
            {
                ArrayList data1 = new ArrayList { advm.productExpeieryByyear[i].Year, advm.productExpeieryByyear[i].Sales };
                data.Add(data1);
            }
            // convert it in json
            string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
            // store it in viewdata/ viewbag
            ViewBag.GVData = new HtmlString(dataStr);
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

        private void ProductMaarks()
        {
            List<PersonProduct> soldProducts = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false && x.Rating != 0).ToList();
            List<Product> products = context.Products.ToList();
            List<PersonProductMark> ppm = context.PersonProductMarks.ToList();
            advm.ProductsMark = new List<ProductMarkViewModel>();


            soldProducts.Select(x => x.UserId);

            List<string> uid = new List<string>();
            for (int i = 0; i < soldProducts.Count(); i++)
            {
                uid.Add(soldProducts[i].UserId);
            }
            List<string> novi = uid.Distinct().ToList();

            for (int i = 0; i < novi.Count; i++)
            {
                ProductMarkViewModel productMark = new ProductMarkViewModel();
                ApplicationUser au = new ApplicationUser();
                au = context.Users.Find(novi[i]);
                productMark.AverageMark = au.GetAverageMark;
                productMark.ProducerFullName = au.GetDisplayName();
                advm.ProductsMark.Add(productMark);
            }


            //for (int i = 0; i < soldProducts.Count(); i++)
            //{
            //    productMark.UserId = soldProducts[i].UserId;
            //    ApplicationUser user = context.Users.Find(productMark.UserId);
            //    productMark.ProducerFullName = user.GetFirstLastName();
            //    productMark.AverageMark = user.GetAverageMark;
            //    advm.ProductsMark.Add(productMark);

            //} cekk vidi ovo ok

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
        //    private void ProductMaarks()
        //    {
        //        List<PersonProduct> soldProducts = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false).ToList();
        //        List<Product> products = context.Products.ToList();
        //        List<PersonProductMark> ppm = context.PersonProductMarks.ToList();
        //        advm.ProductsMark = new List<ProductMarkViewModel>();

        //        ProductMarkViewModel productMark = new ProductMarkViewModel();

        //        for (int i = 0; i < soldProducts.Count(); i++)
        //        {
        //            productMark.UserId = soldProducts[i].UserId;
        //            ApplicationUser user = context.Users.Find(productMark.UserId);
        //            productMark.ProducerFullName = user.GetFirstLastName();
        //            productMark.AverageMark = user.GetAverageMark;
        //            advm.ProductsMark.Add(productMark);

        //        }

        //        ArrayList header = new ArrayList { "Proizvodjac ", "Ocjena:" };
        //        ArrayList data = new ArrayList();
        //        data.Add(header);
        //        for (int i = 0; i < advm.ProductsMark.Count(); i++)
        //        {
        //            ArrayList data1 = new ArrayList { advm.ProductsMark[i].ProducerFullName, advm.ProductsMark[i].AverageMark };
        //            data.Add(data1);
        //        }


        //        // convert it in json
        //        string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
        //        // store it in viewdata/ viewbag
        //        ViewBag.SMDataa = new HtmlString(dataStr);
        //        //TempData["data"] = new HtmlString(dataStr);
        //        //dvm.htmlstringSUPM = ViewBag.Dataa;

        //        //haza

        //    //    List<PersonProduct> soldProducts = context.PersonProducts.Where(x => x.Accepted == true && x.Status == false).ToList();
        //    //    List<Product> products = context.Products.ToList();
        //    //    List<PersonProductMark> ppm = context.PersonProductMarks.ToList();
        //    //    advm.ProductsMark = new List<ProductMarkViewModel>();

        //    //    ProductMarkViewModel productMark = new ProductMarkViewModel();

        //    //    for (int i = 0; i < soldProducts.Count(); i++)
        //    //    {
        //    //        productMark.UserId = soldProducts[i].UserId;
        //    //        ApplicationUser user = context.Users.Find(productMark.UserId);
        //    //        productMark.ProducerFullName = user.GetFirstLastName();
        //    //        productMark.AverageMark = user.GetAverageMark;
        //    //        advm.ProductsMark.Add(productMark);

        //    //    }

        //    //    ArrayList header = new ArrayList { "Proizvodjac ", "Ocjena:" };
        //    //    ArrayList data = new ArrayList();
        //    //    data.Add(header);
        //    //    for (int i = 0; i < advm.ProductsMark.Count(); i++)
        //    //    {
        //    //        ArrayList data1 = new ArrayList { advm.ProductsMark[i].ProducerFullName, advm.ProductsMark[i].AverageMark };
        //    //        data.Add(data1);
        //    //    }


        //    //    // convert it in json
        //    //    string dataStr = JsonConvert.SerializeObject(data, Formatting.None);
        //    //    // store it in viewdata/ viewbag
        //    //    ViewBag.SMDataa = new HtmlString(dataStr);
        //    //    //TempData["data"] = new HtmlString(dataStr);
        //    //    //dvm.htmlstringSUPM = ViewBag.Dataa;

        //    //}
        //}
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

        

    }

}