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
using System.Net.Http;
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UPPHercegovina_Mobile.Views.Producer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNewPersonProductView : Page
    {

        private WebAPIHelper _productsService = new WebAPIHelper("http://localhost:57397", "api/Products");
        private WebAPIHelper _fieldsService = new WebAPIHelper("http://localhost:57397", "api/Fields");
        private WebAPIHelper _personProductService = new WebAPIHelper("http://localhost:57397", "api/PersonProducts");

        private List<FieldsViewModel> userFields;
        private List<ProductsViewModel> products;

        MessageDialog message;
        private bool _openedMenuAnimation = false;

        public AddNewPersonProductView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindData();

        }

        private void BindData()
        {
            var _fieldsResponse = _fieldsService.GetActionResponse("UserFields", GlobalData.Instance.CurrentUser.Id)
                .Content.ReadAsStringAsync().Result;
            userFields = JsonConvert.DeserializeObject<List<FieldsViewModel>>(_fieldsResponse);


            var _productsResponse = _productsService.GetResponse("GetActiveProducts").Content.ReadAsStringAsync().Result;
            products = JsonConvert.DeserializeObject<List<ProductsViewModel>>(_productsResponse);

            BindCombo();

        }

        private void BindCombo()
        {
            //treba najmanje 5 itema da se otvori lijepo
            cbProducts.ItemsSource = products;
            cbProducts.DisplayMemberPath = "Name";
            cbFields.ItemsSource = userFields;
            cbFields.DisplayMemberPath = "Name";

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            PersonProduct personProduct = new PersonProduct();
            personProduct.FieldId = ((FieldsViewModel)cbFields.SelectedValue).Id;
            personProduct.ProductId = ((ProductsViewModel)cbProducts.SelectedValue).Id;
            personProduct.HarvestDate = Convert.ToDateTime(harvestDate.Date.ToString());
            personProduct.ExparationDate = Convert.ToDateTime(expirationDate.Date.ToString());
            personProduct.Neto = neto.Text.ToString();
            personProduct.Bruto = bruto.Text.ToString();
            personProduct.CircaValue = Convert.ToDecimal(expectedValue.Text.ToString());
            personProduct.Damaged = Convert.ToBoolean(damaged.IsChecked);
            personProduct.UserId = GlobalData.Instance.CurrentUser.Id;
            personProduct.Urgently = Convert.ToBoolean(urgently.IsChecked);

            string postResponse = _personProductService.PostResponse(personProduct).StatusCode.ToString();
            if (postResponse == "Created")
            {
                message = new MessageDialog("Uspješno ste dodali proizvod!");
                await message.ShowAsync();

                Frame.Navigate(typeof(ProducersProductView));
            }
            else
            {
                message = new MessageDialog("Došlo je do greške, molimo pokušajte poslije! ");
                await message.ShowAsync();
            }
        }

        private void neto_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        private void Validate()
        {
            if (cbFields.SelectedValue == null || cbProducts.SelectedValue == null || bruto.Text == "" || neto.Text == "" || expectedValue.Text == "")
            {
                btnSave.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnSave.Visibility = Visibility.Visible;
            }
        }

        private void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Validate();
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

        private void productsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProducersProductView));
        }
    }
}
