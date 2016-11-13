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
    public sealed partial class AppointmentCreateView : Page
    {

        private WebAPIHelper appointmentService = new WebAPIHelper("http://localhost:57397", "api/Appointments");
        private WebAPIHelper deliveryService = new WebAPIHelper("http://localhost:57397", "api/Deliveries");

        
        MessageDialog message;

        private bool _openedMenuAnimation = false;
   
        public AppointmentCreateView()
        {
            this.InitializeComponent();
            BindProducts();
        }

        private void BindProducts()
        {
            scrollProducts.ItemsSource = null;
            scrollProducts.ItemsSource = GlobalData.Instance.AcceptedProductsForDelivery;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        #region UI

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

        private void ProductTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalData.Instance.IsDelvieryClicked)
            {
                var product = (sender as ProductDeliveryTile).DataContext as PersonProductViewModel;
                GlobalData.Instance.AcceptedProductsForDelivery.Remove(product);
                GlobalData.Instance.IsDelvieryClicked = false;

                BindProducts();
            }
            else
            {
                var product = (sender as ProductDeliveryTile).DataContext as PersonProductViewModel;
                Frame.Navigate(typeof(ProducerProductDetailsView), product);
            }


        }

        private void Validate()
        {
            if (txtDetails.Text == "")
            {
                btnCreateAppointment.Visibility = Visibility.Collapsed;
            }
            else if(datePickerDeliveryDate.Date >= DateTime.Now)
            {
                btnCreateAppointment.Visibility = Visibility.Visible;
            }

        }

        private void datePickerDeliveryDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            Validate();
        }

        private void txtDetails_LostFocus(object sender, RoutedEventArgs e)
        {
            Validate();
        }

        private async void btnCreateAppointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.DeliveryDate = Convert.ToDateTime(datePickerDeliveryDate.Date.ToString());
            appointment.Details = txtDetails.Text.ToString();
            appointment.Status = true;
            appointment.Canceled = false;
            appointment.Delivered = false;
            appointment.UserId = GlobalData.Instance.CurrentUser.Id;

            var response = appointmentService.PostResponse(appointment).Content.ReadAsStringAsync().Result;
            var ReturnedAppointment = JsonConvert.DeserializeObject<AppointmentPostReturnModel>(response);


            if (ReturnedAppointment.StatusCode == "Created")
            {
                foreach (var item in GlobalData.Instance.AcceptedProductsForDelivery)
                {
                    Delivery delivery = new Delivery();
                    delivery.AppointmentId = ReturnedAppointment.Id;
                    delivery.Status = true;
                    delivery.PersonProductId = item.Id;
                    delivery.Delivered = false;

                    string deliveryresponse = deliveryService.PostResponse(delivery).StatusCode.ToString();

                    if (deliveryresponse != "Created")
                    {
                        message = new MessageDialog("Greška pri doavanju");
                        await message.ShowAsync();
                    }
                }

                GlobalData.Instance.AcceptedProductsForDelivery.Clear();
                

                message = new MessageDialog(String.Format("Uspješno ste kreirali rezervaciju Šifre: {0}, Datuma: {1} !", 
                    ReturnedAppointment.Id, appointment.DeliveryDate.ToString()));
                await message.ShowAsync();

                Frame.Navigate(typeof(ProducersProductView));
            }
            else
            {
                message = new MessageDialog("Molimo pokušajte ponovo!");

            }
        }
    }
}
