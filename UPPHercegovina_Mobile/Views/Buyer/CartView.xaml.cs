using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UPPHercegovina_Mobile.Controls;
using UPPHercegovina_PCL.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class CartView : Page
    {
        private bool _openedMenuAnimation = false;

        public CartView()
        {
            this.InitializeComponent();
            BindData();
        }

        private void BindData()
        {
            scrollProducts.ItemsSource = null;
            scrollProducts.ItemsSource = GlobalData.Instance.ProductsInCart;
        }

        private void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalData.Instance.IsCartClicked)
            {
                BindData();
                GlobalData.Instance.IsCartClicked = false;
            }
            else
            {
                var product = (sender as ProductTile).DataContext as PersonProductViewModel;
                Frame.Navigate(typeof(ProductDetailsView), product);
            }
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

        private void logoutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlobalData.Instance.Clear();
            Frame.Navigate(typeof(Login));
        }

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductsView));
        }

        private void btnReserve_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ReservationView));
        }
    }
}
