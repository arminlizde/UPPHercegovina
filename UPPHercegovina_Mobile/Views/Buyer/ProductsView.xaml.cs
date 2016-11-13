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
using Windows.UI.Popups;
using Windows.UI.Core;
using System.Threading.Tasks;

using System.ComponentModel;

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class ProductsView : Page
    {
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");
        private List<PersonProductViewModel> _products;
        private List<PersonProductViewModel> _filteredProducts;
        private bool _openedMenuAnimation = false;
        private MessageDialog _message;

        public ProductsView()
        {
            this.InitializeComponent();
            _products = new List<PersonProductViewModel>();
            _filteredProducts = new List<PersonProductViewModel>();
            GetData();
            BindData();
        }

        private void GetData()
        {
            var response = _userService.GetResponse().Content.ReadAsStringAsync().Result;
            _products = JsonConvert.DeserializeObject<List<PersonProductViewModel>>(response);
            _filteredProducts = _products;
        }

        private void BindData()
        {
            scrollProducts.ItemsSource = null;
            scrollProducts.ItemsSource = _filteredProducts;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filteredProducts = new List<PersonProductViewModel>();

            _filteredProducts = _products.Where(p => p.Product.StartsWith(txtSearch.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            BindData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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


        private async void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalData.Instance.IsCartClicked)
            {
                BindData();

                if((sender as ProductTile).IsAddedToCart)
                {
                    _message = new MessageDialog("Proizvod dodan u korpu !!!");
                    await _message.ShowAsync();
                }
                GlobalData.Instance.IsCartClicked = false;
            }
            else
            {
                GlobalData.Instance.ShowCartProductDetails = true;
                var product = (sender as ProductTile).DataContext as PersonProductViewModel;
                Frame.Navigate(typeof(ProductDetailsView), product);
            }
        }


        private void newsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BuyerNewsView));
        }

        private void historyBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HistoryView));
        }

        private void profileBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfileView));
        }

        private void buyersBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PricesView));
        }

        private void cartBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CartView));
        }

        private void logoutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlobalData.Instance.Clear();
            Frame.Navigate(typeof(Login));
        }


    }
}
