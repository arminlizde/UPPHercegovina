using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class ProductDetailsView : Page
    {
        private bool _openedMenuAnimation = false;
        private PersonProductViewModel _product;
        private MessageDialog _message;

        public ProductDetailsView()
        {
            this.InitializeComponent();
            _product = new PersonProductViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _product = e.Parameter as PersonProductViewModel;

            this.DataContext = _product;
            if (!GlobalData.Instance.ShowCartProductDetails)
            {
                imgCart.Visibility = Visibility.Collapsed;
                GlobalData.Instance.ShowCartProductDetails = true;
            }

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

        private void cartBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CartView));
        }

        private void logoutBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GlobalData.Instance.Clear();
            Frame.Navigate(typeof(Login));
        }

        private async void gridCart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalData.Instance.ProductsInCart.Where(p => p.Id == _product.Id).Count() == 0)
            {
                GlobalData.Instance.ProductsInCart.Add(_product);
                _message = new MessageDialog("Proizvod dodan u korpu !!!");
                await _message.ShowAsync();
            }
            else
            {
                foreach (var item in GlobalData.Instance.ProductsInCart)
                {
                    if (item.Id == _product.Id)
                    {
                        GlobalData.Instance.ProductsInCart.Remove(item);
                        break;
                    }
                }
            }

            imgCart.Source = new BitmapImage(new Uri(_product.SetCart, UriKind.RelativeOrAbsolute));
        }

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductsView));
        }
    }
}
