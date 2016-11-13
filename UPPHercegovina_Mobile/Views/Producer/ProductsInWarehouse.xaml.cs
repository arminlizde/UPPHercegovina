using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UPPHercegovina_PCL.Util;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using UPPHercegovina_PCL.Models;
using UPPHercegovina_Mobile.Controls;

namespace UPPHercegovina_Mobile.Views.Producer
{
    public sealed partial class ProductsInWarehouse : Page
    {
        private WebAPIHelper _personProductsService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");
        private WebAPIHelper _warehouseService = new WebAPIHelper("http://localhost:57397", "api/Warehouses");

        private List<PersonProductViewModel> _personProducts;
        private List<WarehouseViewModel> _warehouses;


        private bool _openedMenuAnimation = false;

        public ProductsInWarehouse()
        {
            this.InitializeComponent();
            BindCombo();
        }

        private void BindCombo()
        {
            //podaci za listu
            var response = _personProductsService.GetResponse("GetAllActiveProducts").Content.ReadAsStringAsync().Result;
            _personProducts = JsonConvert.DeserializeObject<List<PersonProductViewModel>>(response);

            var wresponse = _warehouseService.GetResponse("GetAll").Content.ReadAsStringAsync().Result;
            _warehouses = JsonConvert.DeserializeObject<List<WarehouseViewModel>>(wresponse);

            WarehouseViewModel warehouse = new WarehouseViewModel() { Id = 0, Name = "SVA SKLADIŠTA" };

            _warehouses.Insert(0, warehouse);

            cmbWarehouse.ItemsSource = _warehouses;
            cmbWarehouse.DisplayMemberPath = "Name";
            //userProducts.ItemsSource = _personProducts; -u listu ubaciti            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindList(0);
        }


        private void CloseMenuPanel()
        {
            ClosingMenuPanel.Begin();
            _openedMenuAnimation = false;
        }

        private void OpenMenuPanel()
        {
            OpeningMenuPanel.Begin();
            _openedMenuAnimation = true;
        }

        private void btnMenu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!_openedMenuAnimation)
                OpenMenuPanel();
            else
                CloseMenuPanel();
        }

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducersProductView));
        }

        private void addNewPersonProdcuctBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddNewPersonProductView));
        }

        private void appointmentCreateBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AppointmentCreateView));
        }

        private void topSoldProductsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TopProductsInWarehouseView));
        }

        private void activeProducts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //ovaj cu view koristit da prikazem sva stanja prizvoda, aktivni, prodani itd..
            Frame.Navigate(typeof(ProducersProductView));
        }

        private void logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlobalData.Instance.Clear();
            Frame.Navigate(typeof(Login));
        }

        private void profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducerProfileView));
        }

        private void gridNews_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducerNewsView));
        }

        private void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var product = (sender as ProductTileBuyer).DataContext as PersonProductViewModel;

            Frame.Navigate(typeof(ProducerProductDetailsView), product);
        }

        private void cmbWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int  warehouseID = ((WarehouseViewModel)cmbWarehouse.SelectedValue).Id;

            BindList(warehouseID);           
        }

        private void BindList(int WarehouseId)
        {
            if (WarehouseId == 0)
            {
                scrollProducts.ItemsSource = _personProducts;

            }
            else
            {
                scrollProducts.ItemsSource = _personProducts.Where(p => p.WarehouseId == WarehouseId);
            }
        }
    }
}
