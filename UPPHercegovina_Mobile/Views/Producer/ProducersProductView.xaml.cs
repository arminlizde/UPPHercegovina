using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UPPHercegovina_Mobile.Controls;
using UPPHercegovina_PCL.Models;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UPPHercegovina_Mobile.Views.Producer
{
    public sealed partial class ProducersProductView : Page
    {
        private WebAPIHelper _productsService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");
        private List<PersonProductViewModel> _products;

        private bool _openedMenuAnimation = false;

        public ProducersProductView()
        {
            this.InitializeComponent();
        }

        #region basicUIParts

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

        private void gridWarehouse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductsInWarehouse));
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //najbolje povuci odjednom sve personproduct od Usera pa ih ovdje hendlaj :) eto me za 5

            BindCombo();
            GetPersonProducts();
        }

        private void GetPersonProducts()
        {
            var _productsResponse = _productsService.GetActionResponse("AllPersonProducts", GlobalData.Instance.CurrentUser.Id).Content.ReadAsStringAsync().Result;
            _products = JsonConvert.DeserializeObject<List<PersonProductViewModel>>(_productsResponse);


            BindScrollViewer(1);
        }

        private void BindScrollViewer(int id)
        {
            switch(id)
            {
                case 1: scrollProducts.ItemsSource = _products.Where(p =>  p.InWarehouse == true).ToList(); break;
                case 2: scrollProducts.ItemsSource = _products.Where(p =>  p.Status == false && p.Accepted == false).ToList(); break;
                case 3: scrollProducts.ItemsSource = _products.Where(p =>  p.Status == false && p.Accepted == true).ToList(); break;
                case 4: Frame.Navigate(typeof(ProcessingProductsView), _products.Where(p => p.Status == true && p.Accepted == true && p.InWarehouse == false).ToList()); break;
                default: break;
            }
        }

        private void BindCombo()
        {
            List<ProductStatus> _status = new List<ProductStatus>();
            ProductStatus inWarehouse = new ProductStatus() {Id = 1,Name="U Skladištu" };
            ProductStatus rejected = new ProductStatus() { Id = 2, Name = "Odbijeni" };
            ProductStatus sold = new ProductStatus() { Id = 3, Name = "Završeni" };
            ProductStatus acceptedNotDelivered = new ProductStatus() { Id = 4, Name = "Prihvaćeni" };
            _status.Add(inWarehouse);
            _status.Add(rejected);
            _status.Add(sold);
            _status.Add(acceptedNotDelivered);


            cmbProductStatus.ItemsSource = _status;
            cmbProductStatus.DisplayMemberPath = "Name";
        }

        private void cmbProductStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindScrollViewer(((ProductStatus)cmbProductStatus.SelectedValue).Id);
        }

        private void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var product = (sender as ProductTileBuyer).DataContext as PersonProductViewModel;
            
            Frame.Navigate(typeof(ProducerProductDetailsView), product);
        }
    }
}
