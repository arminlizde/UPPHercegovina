using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UPPHercegovina_PCL.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UPPHercegovina_Mobile.Views.Producer
{

    public sealed partial class ProducerNewsDetailsView : Page
    {
        private bool _openedMenuAnimation = false;
        private PostViewModel _post;

        public ProducerNewsDetailsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _post = e.Parameter as PostViewModel;
            FillView();
        }

        private async void FillView()
        {
            txtAuthor.Text = _post.AuthorName;
            txtDate.Text = _post.PostDateOnlyDate;
            txtTitle.Text = StripHTML(_post.Title);
            // txtText.Text = Html2XamlConverter.Convert2Xaml(_post.Text);
            txtText.Text = StripHTML(_post.Text);



            if (_post.Picture != null)
            {
                var memStream = new MemoryStream(_post.Picture);
                memStream.Position = 0;
                var bitmap = new BitmapImage();

                await bitmap.SetSourceAsync(memStream.AsRandomAccessStream());

                imgNews.Source = bitmap;
            }


        }

        public static string StripHTML(string input)
        {
            string noHTML = Regex.Replace(input, @"<[^>]+>|&nbsp;", "").Trim();
            return Regex.Replace(noHTML, @"\s{2,}", " ");

            //return Regex.Replace(input, "<.*?>", String.Empty);
            // return Regex.Replace(input, "(?<=<[^>]*)&nbsp;", String.Empty);
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
