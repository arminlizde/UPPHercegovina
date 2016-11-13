using System.Linq;
using UPPHercegovina_PCL.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UPPHercegovina_Mobile.Controls
{
    public sealed partial class ProductDeliveryTile : UserControl
    {
        public bool IsAddedForDelivery = false;

        public ProductDeliveryTile()
        {
            this.InitializeComponent();
        }

        private void imgCart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = false;
            var product = (PersonProductViewModel)this.DataContext;

            if (GlobalData.Instance.AcceptedProductsForDelivery.Where(p => p.Id == product.Id).Count() == 0)
            {
                GlobalData.Instance.AcceptedProductsForDelivery.Add(product);
                IsAddedForDelivery = true;
            }
            else
            {
                foreach (var item in GlobalData.Instance.AcceptedProductsForDelivery)
                {
                    if (item.Id == product.Id)
                    {
                        GlobalData.Instance.AcceptedProductsForDelivery.Remove(item);
                        break;
                    }
                }
            }

            GlobalData.Instance.IsDelvieryClicked = true;
        }
    }
}
