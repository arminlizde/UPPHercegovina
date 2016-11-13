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

namespace UPPHercegovina_Mobile.Views.Buyer
{
    public sealed partial class HistoryView : Page
    {
        private bool _openedMenuAnimation = false;
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/BuyerRequests");
        private List<BuyerRequestViewModel> _requests;
        private List<ComboListModel> _comboList;

        public HistoryView()
        {
            this.InitializeComponent();

            openingHeight.Value = Window.Current.Bounds.Height;
            openingWidth.Value = Window.Current.Bounds.Width;

            _requests = new List<BuyerRequestViewModel>();
            _comboList = new List<ComboListModel>();

            GetData();
            BindData();
            BindCombo();
        }

        private void BindCombo()
        {
            _comboList.Add(new ComboListModel() { Display = "SVI", Value = "SVI" });
            _comboList.Add(new ComboListModel() { Display = "AKTIVNI", Value = "AKTIVNI" });
            _comboList.Add(new ComboListModel() { Display = "NEAKTIVNI", Value = "NEAKTIVNI" });
            _comboList.Add(new ComboListModel() { Display = "ODBIJENI", Value = "ODBIJENI" });

            comboFilter.ItemsSource = _comboList;
            comboFilter.DisplayMemberPath = "Value";

        }

        private void GetData()
        {
            var response = _userService
                .GetActionResponse("User", GlobalData.Instance.CurrentUser.Id).Content.ReadAsStringAsync().Result;
            _requests = JsonConvert.DeserializeObject<List<BuyerRequestViewModel>>(response);
        }

        private void BindData()
        {
            scrollRequests.ItemsSource = _requests;
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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        private void newsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BuyerNewsView));
        }

        private void closeBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClosingInformationPanel.Begin();
        }

        private void ReservationTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpeningInformationPanel.Begin();

            var reservation = (sender as ReservationTile).DataContext as BuyerRequestViewModel;
            BindInformationTile(reservation);
        }

        private void BindInformationTile(BuyerRequestViewModel reservation)
        {
            txtRequestId.Text = reservation.Id.ToString();
            txtDetails.Text = reservation.Details;
            txtAccepted.Text = reservation.AcceptedString;
            txtDateOfCreation.Text = String.Format("{0}/{1}/{2}", reservation.DateOfCreation.Day, reservation.DateOfCreation.Month, reservation.DateOfCreation.Year);
            txtPickedUp.Text = reservation.PickedUpString;
            txtPickUpDate.Text = reservation.PickUpOnlyDate;
            txtStatus.Text = reservation.StatusString;

            scrollProducts.ItemsSource = reservation.Products;
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpeningInformationPanel.Begin();
        }

        private void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var product = (sender as Product_MoreInfoTile).DataContext as PersonProductViewModel;
            GlobalData.Instance.ShowCartProductDetails = false;
            Frame.Navigate(typeof(ProductDetailsView), product);
        }

        private void comboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(((ComboListModel)comboFilter.SelectedValue).Value)
            {
                case "SVI": scrollRequests.ItemsSource = _requests; break;
                case "AKTIVNI": scrollRequests.ItemsSource = _requests.Where(r => r.Status == true); break;
                case "NEAKTIVNI": scrollRequests.ItemsSource = _requests.Where(r => r.Status == false && r.Accepted == true); break;
                case "ODBIJENI": scrollRequests.ItemsSource = _requests.Where(r => r.Status == false && r.Accepted == false); break;
                default: break;
                
            }
        }
    }
}
