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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace UPPHercegovina_Mobile.Controls
{
    public sealed partial class MsgBoxFadingTile : Page
    {
        public MsgBoxFadingTile()
        {
            this.InitializeComponent();
        }


        public MsgBoxFadingTile(string text)
        {
            this.InitializeComponent();

            txtMessage.Text = text;
        }

        public void CloseMessageBox()
        {
            ClosingMsgBox.Begin();
        }
    }
}
