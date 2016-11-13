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
    public sealed partial class ReservationView : Page
    {
        private bool _openedMenuAnimation = false;
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/BuyerRequests");
        private MessageDialog _message;

        public ReservationView()
        {
            this.InitializeComponent();
            BindData();
        }

        private void BindData()
        {
            scrollProducts.ItemsSource = null;
            scrollProducts.ItemsSource = GlobalData.Instance.ProductsInCart;
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

        private async void btnReserve_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BuyerRequestViewModel request = new BuyerRequestViewModel();
            request.BuyerId = GlobalData.Instance.CurrentUser.Id;
            request.Details = txtDetails.Text;
            request.DateOfCreation = DateTime.Now;
            request.PickUpDate = datePickerPickUpDate.Date.DateTime;
            request.Status = true;
            request.Accepted = false;
            request.PickedUp = false;
            request.Products = GlobalData.Instance.ProductsInCart;


            var response = _userService.PostResponse(request).Content.ReadAsStringAsync().Result;

            _message = new MessageDialog("Rezervisali ste proizvode !!!");
            await _message.ShowAsync();

            GlobalData.Instance.ProductsInCart.Clear();

            Frame.Navigate(typeof(ProductsView));
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
    }
}
