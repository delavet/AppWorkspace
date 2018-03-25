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
using codeRetrievalApp.Lib;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class InputControl : UserControl
    {
        private bool showing = false;
        public InputControl()
        {
            this.InitializeComponent();
            STRBDrt.Begin();
        }
        public InputControl(String inputStr)
        {
            this.InitializeComponent();
            TXTBLKinput.Visibility = Visibility.Collapsed;
            TXTBXinput.Visibility = Visibility.Visible;
            CTRLlv.Visibility = Visibility.Visible;
            CTRLas.Visibility = Visibility.Visible;
            TXTBXinput.Text = inputStr;
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        public void changeForm(String inputStr)
        {
            TXTBXinput.Text = inputStr;
            STRBDrt1.Begin();
        }

        private void STRBDrt1_Completed(object sender, object e)
        {
            TXTBLKinput.Visibility = Visibility.Collapsed;
            TXTBXinput.Visibility = Visibility.Visible;
            CTRLlv.Visibility = Visibility.Visible;
            CTRLas.Visibility = Visibility.Visible;
            STRBDrt2.Begin();
        }
    }
}
