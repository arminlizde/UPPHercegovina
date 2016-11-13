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
    public sealed partial class PriceTile : UserControl
    {
        private PriceViewModel _price;

        public PriceTile()
        {
            this.InitializeComponent();
            _price = new PriceViewModel();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            _price = (PriceViewModel)this.DataContext;
            txtMax.Text = "Do: " + _price.MaxValue;
            txtMin.Text = "Od: " + _price.MinValue;
            txtAverage.Text = "Prosječno: " + _price.AverageValue;
        }
    }
}
