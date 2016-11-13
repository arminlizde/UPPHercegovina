using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace UPPHercegovina_Mobile.Views.Producer
{
    public sealed partial class ProducerProfileView : Page
    {

        private bool _openedMenuAnimation = false;
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/AspNetUsers");
        private WebAPIHelper _personProductsService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");

        AspNetUserViewModel _user = GlobalData.Instance.CurrentUser;

        public ProducerProfileView()
        {
            this.InitializeComponent();
            GetData();
            BindData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void GetData()
        {
            var response = _personProductsService.GetActionResponse("GetAllPersonProducts", _user.Id).Content.ReadAsStringAsync().Result;
            var averageMark = _userService.GetActionResponse("GetAverageMark", _user.Id).Content.ReadAsStringAsync().Result;
            List<PersonProductViewModel> _productsAll = JsonConvert.DeserializeObject<List<PersonProductViewModel>>(response);

            _user.BrutoAddedKg = _user.BrutoSoldKg = _user.NetoAddedKg = _user.NetoSoldKg = 0;
            _user.AverageMark = averageMark;

            foreach (var item in _productsAll)
            {
                if (item.Status == false)
                {
                    _user.BrutoAddedKg += Convert.ToInt32(item.Bruto);
                    _user.NetoAddedKg += Convert.ToInt32(item.Neto);
                }
                else
                {
                    _user.BrutoSoldKg += Convert.ToInt32(item.Bruto);
                    _user.NetoSoldKg += Convert.ToInt32(item.Neto);
                }
            }
        }

        private void BindData()
        {
            this.DataContext = GlobalData.Instance.CurrentUser;
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
    }
}
