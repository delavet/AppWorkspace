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
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class LevelControl : UserControl
    {
        private Boolean shown = false;
        private Color Lv1c = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        private Color Lv2c = Color.FromArgb(0xff, 0x32, 0xa1, 0xb9);
        private Color Lv3c = Color.FromArgb(0xff, 0x43, 0xb6, 0x6d);
        private Color Lv4c = Color.FromArgb(0xff, 0xc8, 0xcf, 0x3d);
        private Color Lv5c = Color.FromArgb(0xff, 0xf1, 0x15, 0x15);
        private int _curLv;
        public int CurrentLv
        {
            get { return _curLv; }
            set { _curLv = value; }
        }
        
        public LevelControl()
        {
            this.InitializeComponent();
        }

        private void lv1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BGlv1.Color = Lv1c;
            BGlv2.Color = Lv2c;
            BGlv3.Color = Lv3c;
            BGlv4.Color = Lv4c;
            BGlv5.Color = Lv5c;
        }

        private void lv1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!shown)
            {
                shown = true;
                STRBDshow.Begin();
            }
            else
            {
                shown = false;
                STRBDunshow.Begin();
            }
        }

        private void lv2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BGlv1.Color = Lv2c;
            BGlv2.Color = Lv2c;
            BGlv3.Color = Lv3c;
            BGlv4.Color = Lv4c;
            BGlv5.Color = Lv5c;
        }

        private void lv3_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BGlv1.Color = Lv3c;
            BGlv2.Color = Lv3c;
            BGlv3.Color = Lv3c;
            BGlv4.Color = Lv4c;
            BGlv5.Color = Lv5c;
        }

        private void lv4_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BGlv1.Color = Lv4c;
            BGlv2.Color = Lv4c;
            BGlv3.Color = Lv4c;
            BGlv4.Color = Lv4c;
            BGlv5.Color = Lv5c;
        }

        private void lv5_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BGlv1.Color = Lv5c;
            BGlv2.Color = Lv5c;
            BGlv3.Color = Lv5c;
            BGlv4.Color = Lv5c;
            BGlv5.Color = Lv5c;
        }

        private void lv5_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (shown)
            {
                shown = false;
                STRBDunshow.Begin();
            }
        }

        private void lv2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (shown)
            {
                shown = false;
                STRBDunshow.Begin();
            }
        }

        private void lv3_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (shown)
            {
                shown = false;
                STRBDunshow.Begin();
            }
        }

        private void lv4_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (shown)
            {
                shown = false;
                STRBDunshow.Begin();
            }
        }
    }
}
