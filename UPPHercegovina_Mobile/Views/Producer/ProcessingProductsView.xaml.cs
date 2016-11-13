using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UPPHercegovina_Mobile.Controls;
using UPPHercegovina_PCL.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProcessingProductsView : Page
    {
        private List<PersonProductViewModel> _products;
        private bool _openedMenuAnimation = false;
        private MessageDialog _message;

        public ProcessingProductsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _products = e.Parameter as List<PersonProductViewModel>;
            scrollProducts.ItemsSource = _products;
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

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducersProductView));
        }
        #endregion

        private async void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalData.Instance.IsDelvieryClicked)
            {
                BindData();

                if ((sender as ProductDeliveryTile).IsAddedForDelivery)
                {
                    _message = new MessageDialog("Proizvod dodan za dostavu !!!");
                    await _message.ShowAsync();
                }
                GlobalData.Instance.IsDelvieryClicked = false;
            }
            else
            {
                var product = (sender as ProductDeliveryTile).DataContext as PersonProductViewModel;
                Frame.Navigate(typeof(ProducerProductDetailsView), product);
            }
        }

        private void BindData()
        {
            scrollProducts.ItemsSource = null;
            scrollProducts.ItemsSource = _products;
        }
    }
}
