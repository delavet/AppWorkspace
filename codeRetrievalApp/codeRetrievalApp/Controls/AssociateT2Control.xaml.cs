using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class AssociateT2Control : UserControl
    {
        private bool rt;
        private Boolean _important;
        public Boolean Important
        {
            get
            {
                return _important;
            }
            set
            {
                _important = value;
                if (!_important)
                {
                    BDcore.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                }
                else
                {
                    BDcore.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xe7, 0x48, 0x56));
                }
            }
        }
        public event AssociateKeyWordsHandler ShowAssociates;
        public String KeyWord
        {
            get
            {
                return TXTBLLasso.Text;
            }
            set
            {
                TXTBLLasso.Text = value;
            }
        }

        public AssociateT2Control()
        {
            this.InitializeComponent();
        }

        public AssociateT2Control(String kw, bool rightEnable=true)
        {
            this.InitializeComponent();
            KeyWord = kw;
            rt = rightEnable;
        }


        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SCALEroot.CenterX = GRIDroot.ActualWidth/2;
            SCALEroot.CenterY = GRIDroot.ActualHeight/2;
            STRBDenter.Begin();
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SCALEroot.CenterX = GRIDroot.ActualWidth / 2;
            SCALEroot.CenterY = GRIDroot.ActualHeight / 2;
            STRBDexit.Begin();
        }

        private void GRIDroot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(ShowAssociates!=null) ShowAssociates(this, KeyWord);
        }

        private void GRIDroot_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
           if(rt) Important = !Important;
        }
    }
}
