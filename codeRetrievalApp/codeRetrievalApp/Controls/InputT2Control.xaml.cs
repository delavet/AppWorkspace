using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class InputT2Control : UserControl
    {
        public object o;
        public bool q = false;
        private KeywordControl cur = null;
        public event AssociateKeyWordsHandler ShowAssociates;

        public InputT2Control()
        {
            this.InitializeComponent();
            ControlInitialize();
        }

        public void ControlInitialize()
        {
            cur = KWfirst;
            cur.SetFirst();
            cur.SetFocus();
        }

        private void KeywordControl_ShowAssociates(FrameworkElement kwItem,String keyword)
        {
            ShowAssociates(kwItem, keyword);
            /*
            Button button1 = new Button();
            button1.FontSize = 15;
            button1.Content = "test1";
            button1.Margin = new Thickness(3);
            button1.Click += Button_Click;
            button1.Style = (Style)Application.Current.Resources["ButtonRevealStyle"];
            Button button2 = new Button();
            button2.FontSize = 15;
            button2.Content = "test2";
            button2.Margin = new Thickness(3);
            button2.Click += Button_Click;
            button2.Style = (Style)Application.Current.Resources["ButtonRevealStyle"];
            WPPNassociate.Children.Clear();
            WPPNassociate.Children.Add(button1);
            WPPNassociate.Children.Add(button2);
            */
        }
        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            String str = btn.Content as String;
            TextBlock tempBlock = new TextBlock();
            tempBlock.Text = str;
            tempBlock.Margin = new Thickness(5, 1, 5, 1);
            WPPNdecide.Children.Add(tempBlock);
        }
        */

        private void KeywordControl_KeywordComplete()
        {
            if (cur!=null)
            {
                cur.ReleaseFocus();
            }
            
            KeywordControl tempKW = new KeywordControl();
            
            tempKW.ShowAssociates += KeywordControl_ShowAssociates;
            tempKW.KeywordComplete += KeywordControl_KeywordComplete;
            cur = tempKW;
            WPPNinput.Children.Add(tempKW);
            tempKW.SetFocus();
        }

        public void quitLead()
        {
            lock (o)
            {
                q = true;
            }
        }

        public void reset()
        {
            WPPNinput.Children.Clear();
        }

        public async void lead()
        {
            while (true)
            {
                bool quit;
                lock (o)
                {
                    quit = q;
                }
                if (q) break;
                reset();
                cur.input("w");
                Thread.Sleep(200);
                cur.input("h");
                Thread.Sleep(200);
                cur.input("a");
                Thread.Sleep(200);
                cur.input("t");
                Thread.Sleep(200);
                KeywordControl_KeywordComplete();
                cur.input("i");
                Thread.Sleep(200);
                cur.input("s");
                Thread.Sleep(200);
                KeywordControl_KeywordComplete();
                cur.input("t");
                Thread.Sleep(200);
                cur.input("h");
                Thread.Sleep(200);
                cur.input("i");
                Thread.Sleep(200);
                cur.input("s");
                Thread.Sleep(200);
            }
            reset();
        }
    }
}
