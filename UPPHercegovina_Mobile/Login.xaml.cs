using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UPPHercegovina_PCL.Util;
using Newtonsoft.Json;
using UPPHercegovina_PCL.Models;
using Windows.UI.Popups;
using System.Threading.Tasks;

using System;
using UPPHercegovina_Mobile.Views.Buyer;
using UPPHercegovina_Mobile.Views.Producer;

namespace UPPHercegovina_Mobile
{
    public sealed partial class Login : Page
    {
        private WebAPIHelper _userService = new WebAPIHelper("http://localhost:57397", "api/AspNetUsers");
        private MessageDialog _messageBox;

        public Login()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private async void loginbtn_Click(object sender, RoutedEventArgs e)
        {
            inputPassword.Password = "Jovan1@Jovan.com";
            inputEmail.Text = "Jovan1@Jovan.com";

            if (!String.IsNullOrEmpty(inputPassword.Password.ToString()))
            {
                string password = String.Format("{0}/", inputPassword.Password.ToString());

                var response = _userService
                    .GetActionResponse("Login", inputEmail.Text.ToString(), password)
                    .Content.ReadAsStringAsync().Result;

                var user = JsonConvert.DeserializeObject<AspNetUserViewModel>(response);

                if (!user.Error)
                {
                    if (user != null)
                    {
                        GlobalData.Instance.CurrentUser = user;
                        _messageBox = new MessageDialog(String.Format("Dobrodošli {0}", GlobalData.Instance.CurrentUser.FullName));

                        await _messageBox.ShowAsync();

                        if (user.Role == "Korisnik")
                            Frame.Navigate(typeof(ProducerNewsView));
                        else
                            Frame.Navigate(typeof(BuyerNewsView));

                    }
                }
                else
                {
                    _messageBox = new MessageDialog("Pogrešna lozinka ili email !");
                    await _messageBox.ShowAsync();
                }
            }
            else
            {
                _messageBox = new MessageDialog("Molimo unesite lozinku !!!");
                await _messageBox.ShowAsync();
            }
        }

    }
        //var response = userService.GetActionResponse("Login", inputIme.Text.ToString(), inputPass.Password.ToString())
        //    .Content.ReadAsAsync<string>().Result;


        //var response = userService.GetResponse()
        //        .Content.ReadAsStringAsync().Result;

        //var x = JsonConvert.DeserializeObject<List<PersonProduct>>(response);

        // var x = userService.GetActionResponse().Content.ReadAsStringAsync().Result;

        // var x = response;
}
