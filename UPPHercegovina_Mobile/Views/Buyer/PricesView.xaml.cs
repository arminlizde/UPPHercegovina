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
using Newtonsoft.Json;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class PricesView : Page
    {
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");
        private List<PriceViewModel> _prices;
        private bool _openedMenuAnimation = false;

        public PricesView()
        {
            this.InitializeComponent();
            _prices = new List<PriceViewModel>();
            GetData();
            BindData();
        }

        private void BindData()
        {
            scrollPrice.ItemsSource = _prices;
        }

        private void GetData()
        {
            var response = _userService
                    .GetResponse("Pricelist").Content.ReadAsStringAsync().Result;
            _prices = JsonConvert.DeserializeObject<List<PriceViewModel>>(response);
            
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

        private void historyBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HistoryView));
        }

        private void profileBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfileView));
        }

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductsView));
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void newsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BuyerNewsView));
        }
    }
}
