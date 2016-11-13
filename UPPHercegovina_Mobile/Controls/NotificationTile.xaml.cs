using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class NotificationTile : UserControl
    {
        public NotificationTile()
        {
            this.InitializeComponent();
        }

        public NotificationTile(string message)
        {
            txtMessage.Text = message;
            this.InitializeComponent();
        }

        public void CloseMessageBox()
        {
            ClosingMsgBox.Begin();
        }
    }
}
