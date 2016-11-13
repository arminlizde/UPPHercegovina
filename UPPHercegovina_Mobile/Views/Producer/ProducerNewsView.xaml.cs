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
using Windows.UI.Core;
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
    public sealed partial class ProducerNewsView : Page
    {
        private bool _openedMenuAnimation = false;
        private WebAPIHelper _postService = new WebAPIHelper("http://localhost:57397", "api/Posts");
        private List<PostViewModel> _news;
        private MessageDialog _messageBox;

        public ProducerNewsView()
        {
            this.InitializeComponent();
            _news = new List<PostViewModel>();
            GlobalData.Instance.ProducersNotifications += ProducerCheckForNotifications;
            GlobalData.Instance.ApplicationStarted = false;
            GetData();
            BindData();
        }

        private async void ProducerCheckForNotifications()
        {
            if (GlobalData.Instance.NumberOfAcceptedProductsChanged)
            {
                await Windows.ApplicationModel.Core.CoreApplication
                    .MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    WebAPIHelper _productService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");

                    var response = _productService
                        .GetActionResponse("GetAcceptedProductsCount", GlobalData.Instance.CurrentUser.Id)
                        .Content.ReadAsStringAsync().Result;

                    List<PersonProductViewModel> products = new List<PersonProductViewModel>();
                    products = JsonConvert.DeserializeObject<List<PersonProductViewModel>>(response);

                    string productsIdMsgBox = "";

                    foreach (var item in products)
                    {
                        productsIdMsgBox += item.Id + System.Environment.NewLine;
                    }

                    _messageBox = new MessageDialog("Proizvodi prihvaćeni!" 
                        + System.Environment.NewLine + "Šifre proizvoda: " 
                        + System.Environment.NewLine + productsIdMsgBox);

                    ShowMessage();
                    GlobalData.Instance.NumberOfNotificationChanged = false;
                }
                );
            }
        }

        private async void ShowMessage()
        {
            await _messageBox.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void GetData()
        {
            var response = _postService.GetResponse().Content.ReadAsStringAsync().Result;
            _news = JsonConvert.DeserializeObject<List<PostViewModel>>(response);
        }

        private void BindData()
        {
            scrollNews.ItemsSource = _news;
        }

        private void PostTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var news = (sender as PostTile).DataContext as PostViewModel;
            Frame.Navigate(typeof(ProducerNewsDetailsView), news);
        }

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducersProductView));
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
    }
}
