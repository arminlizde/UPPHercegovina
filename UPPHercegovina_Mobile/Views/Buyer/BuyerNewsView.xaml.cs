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
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using UPPHercegovina_Mobile.Controls;
using Windows.UI.Popups;
using Windows.UI.Core;

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class BuyerNewsView : Page
    {
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/Posts");
        private List<PostViewModel> _news;
        private bool _openedMenuAnimation = false;

        //in every view
        private MessageDialog _messageBox;


        public BuyerNewsView()
        {
            this.InitializeComponent();
            _news = new List<PostViewModel>();

            GlobalData.Instance.BuyersNotifications += BuyerCheckForNotifications;
            GetData();
            BindData();
            GlobalData.Instance.ApplicationStarted = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        public async void BuyerCheckForNotifications()
        {
            if (GlobalData.Instance.NumberOfNotificationChanged)
            {
                await Windows.ApplicationModel.Core.CoreApplication
                    .MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    _messageBox = new MessageDialog("Imate novu prihvaćenu rezervaciju !!! ");

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


        private void GetData()
        {
            var response = _userService.GetResponse().Content.ReadAsStringAsync().Result;
            _news = JsonConvert.DeserializeObject<List<PostViewModel>>(response);
        }

        private void BindData()
        {
            scrollNews.ItemsSource = _news;
        }


        private void PostTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var news = (sender as PostTile).DataContext as PostViewModel;
            Frame.Navigate(typeof(NewsDetailsView), news);
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
