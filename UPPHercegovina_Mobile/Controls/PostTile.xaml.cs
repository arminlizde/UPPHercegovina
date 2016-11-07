using System.IO;
using UPPHercegovina_PCL.Models;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System;

namespace UPPHercegovina_Mobile.Controls
{
    public sealed partial class PostTile : UserControl
    {
        public PostTile()
        {
            this.InitializeComponent();

        }

        private async void gridPostTile_Loaded(object sender, RoutedEventArgs e)
        {
            var x = (PostViewModel)this.DataContext;


            if (x.Picture == null)
                return;
            var memStream = new MemoryStream(x.Picture);
            memStream.Position = 0;
            var bitmap = new BitmapImage();

            var y = memStream.AsRandomAccessStream();
            await bitmap.SetSourceAsync(y);

            imgNews.Source = bitmap;
        }


    }
}
