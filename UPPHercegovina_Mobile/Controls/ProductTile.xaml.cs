using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UPPHercegovina_PCL.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UPPHercegovina_Mobile.Controls
{
    public sealed partial class ProductTile : UserControl
    {

        public bool IsAddedToCart = false;

        public ProductTile()
        {
            this.InitializeComponent();
        }

        private void imgCart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = false;
            var product = (PersonProductViewModel)this.DataContext;

            if (GlobalData.Instance.ProductsInCart.Where(p => p.Id == product.Id).Count() == 0)
            {
                GlobalData.Instance.ProductsInCart.Add(product);
                IsAddedToCart = true;
            }
            else
            {
                foreach (var item in GlobalData.Instance.ProductsInCart)
                {
                    if (item.Id == product.Id)
                    {
                        GlobalData.Instance.ProductsInCart.Remove(item);
                        break;
                    }
                }
            }

            GlobalData.Instance.IsCartClicked = true;
        }
    }
}
