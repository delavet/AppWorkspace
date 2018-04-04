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
using codeRetrievalApp.Lib;
using Windows.System;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class KeywordControl : UserControl
    {
        public bool leading = false;
        public bool Leading
        {
            get
            {
                return leading;
            }
            set
            {
                leading = value;
            }
        }
        private bool focused = false;
        public String Keyword
        {
            get
            {
                return TXTBLKkword.Text;
            }
            set
            {
                TXTBXkword.Text = value;
                TXTBLKkword.Text = value;
            }
        }

        public event AssociateKeyWordsHandler ShowAssociates;
        public event KeywordInputCompleteHandler KeywordComplete;

        public KeywordControl()
        {
            this.InitializeComponent();
        }

        private void TXTBXkword_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach(char ch in TXTBXkword.Text)
            {
                if (!Char.IsLetterOrDigit(ch))
                {
                    TXTBXkword.Text = TXTBXkword.Text.Remove(TXTBXkword.Text.IndexOf(ch),1);
                }
            }
            TXTBLKkword.Text = TXTBXkword.Text;
        }

        private void BTNcore_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetFocus();
        }

        private void BTNcore_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ReleaseFocus();
        }

        public void SetFirst()
        {
            TXTBXkword.PlaceholderText = "input you description to start search";
        }

        public void SetFocus()
        {
            if (focused) return;
            focused = true;
            BDbg.Opacity = 0.9;
            STKPNcore.Visibility = Visibility.Visible;
            TXTBLKkword.Visibility = Visibility.Collapsed;
            try
            {
                TXTBXkword.Focus(FocusState.Pointer);
            }
            catch
            {
                
            }
        }

        public void ReleaseFocus()
        {
            if (!focused) return;
            focused = false;
            BDbg.Opacity = 0;
            TXTBLKkword.Visibility = Visibility.Visible;
            STKPNcore.Visibility = Visibility.Collapsed;
        }


        private void TXTBXkword_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space||e.Key==VirtualKey.Enter)
            {
                if (KeywordComplete != null)
                {
                    KeywordComplete();
                }
            }
            else if (e.Key == VirtualKey.Up)
            {
                update();
            }
            else if (e.Key == VirtualKey.Down)
            {
                dissdate();
            }
            
        }

        private void update()
        {
            BDcore.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x00, 0x00));
        }

        private void dissdate()
        {
            BDcore.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            
            //SetFocus();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (leading) return;
            ReleaseFocus();
        }


        private void BTNhint_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Keyword != "" && Keyword != null)
            {
                if (ShowAssociates != null)
                {
                    ShowAssociates(this,Keyword);
                }
            }
        }

        private void BTNhint_GotFocus(object sender, RoutedEventArgs e)
        {
            SetFocus();
        }

        public void input(String str)
        {
            TXTBXkword.Text += str;
        }

        public void lead1()
        {
            SetFocus();
            TXTBXkword.Text = "java";
            STRBDlead1.Begin();
        }

        public void quitLead1()
        {
            STRBDlead1.Stop();
        }

        private void STRBDlead1_Completed(object sender, object e)
        {
            STRBDlead1.Begin();
        }

        public void lead2()
        {
            STRBDlead2.Begin();
        }

        public void quitLead2()
        {
            STRBDlead2.Stop();
            fgTXT.Color = Color.FromArgb(0xff, 1, 1, 1);
        }

        private void STRBDlead2_Completed(object sender, object e)
        {
            STRBDlead2.Begin();
        }

        public void reset()
        {
            TXTBXkword.Text = "";
        }
    }
}
