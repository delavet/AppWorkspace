using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
    
    public sealed partial class InputT2Control : UserControl
    {
        public object o;
        public bool q = false;
        private KeywordControl cur = null;
        public event AssociateKeyWordsHandler ShowAssociates;
        private Style FlyoutStyle = new Style(typeof(FlyoutPresenter));
        private String _query;
        public String Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
            }
        }

        public InputT2Control()
        {
            this.InitializeComponent();
            ControlInitialize();
        }

        public void ControlInitialize()
        {
            Query = "";
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
            KWfirst.Leading = false;
            if (cur!=null)
            {
                cur.ReleaseFocus();
                Query += " " + cur.Keyword;
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

        public void lead()
        {
            cur.Leading = true;
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = FlyoutStyle;
            StackPanel tempPanel = new StackPanel();
            TextBlock tempTitle = new TextBlock(), tempContent = new TextBlock();
            Button BTNnext = new Button();
            BTNnext.Content = "next";
            BTNnext.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            BTNnext.Click += LeadNext;
            BTNnext.HorizontalAlignment = HorizontalAlignment.Right;
            tempTitle.FontSize = 24;
            tempContent.FontSize = 15;
            tempTitle.Text = "GENERATE YOUR QUERY";
            tempContent.Text = "Input your query with this search box on top";
            tempContent.TextWrapping = TextWrapping.WrapWholeWords;
            tempContent.MaxWidth = 350;
            tempPanel.Children.Add(tempTitle);
            tempPanel.Children.Add(tempContent);
            tempPanel.Children.Add(BTNnext);
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(this, fly);
            Flyout.ShowAttachedFlyout(this);
        }

        private void LeadNext(object sender, RoutedEventArgs e)
        {
            cur.lead1();
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = FlyoutStyle;
            StackPanel tempPanel = new StackPanel();
            TextBlock tempTitle = new TextBlock(), tempContent = new TextBlock();
            Button BTNnext = new Button();
            BTNnext.Content = "next";
            BTNnext.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            BTNnext.Click += LeadNext1;
            BTNnext.HorizontalAlignment = HorizontalAlignment.Right;
            tempTitle.FontSize = 20;
            tempContent.FontSize = 15;
            tempContent.TextWrapping = TextWrapping.WrapWholeWords;
            tempContent.MaxWidth = 350;
            tempTitle.Text = "IMPORTANCE LEVEL OF THE KEYWORD";
            tempContent.Text = "When a keyword is selected, use key 'up' and 'down' " +
                                              "to change this keyword's level of importance; the keyword"+
                                              "turn to red to reveal that its way more important in this query" +
                                              "than other keyword";
            tempPanel.Children.Add(tempTitle);
            tempPanel.Children.Add(tempContent);
            tempPanel.Children.Add(BTNnext);
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(cur, fly);
            Flyout.ShowAttachedFlyout(cur);
            
        }

        private void LeadNext1(object sender, RoutedEventArgs e)
        {
            cur.quitLead1();
            cur.lead2();
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = FlyoutStyle;
            StackPanel tempPanel = new StackPanel();
            TextBlock tempTitle = new TextBlock(), tempContent = new TextBlock();
            Button BTNnext = new Button();
            BTNnext.Content = "next";
            BTNnext.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            BTNnext.Click += LeadNext2;
            BTNnext.HorizontalAlignment = HorizontalAlignment.Right;
            tempTitle.FontSize = 24;
            tempContent.FontSize = 15;
            tempTitle.Text = "WANT A HINT?";
            tempContent.TextWrapping = TextWrapping.WrapWholeWords;
            tempContent.MaxWidth = 350;
            tempContent.Text = "Click this 'hint' button to get some hint about the keyword selected," +
                                              "you would get some keyword in same topic, which may help you generate" +
                                              "your query more easily";
            tempPanel.Children.Add(tempTitle);
            tempPanel.Children.Add(tempContent);
            tempPanel.Children.Add(BTNnext);
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(cur, fly);
            Flyout.ShowAttachedFlyout(cur);
            
        }

        private void LeadNext2(object sender, RoutedEventArgs e)
        {
            cur.quitLead2();
            cur.reset();
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = FlyoutStyle;
            StackPanel tempPanel = new StackPanel();
            TextBlock tempTitle = new TextBlock(), tempContent = new TextBlock();
            tempTitle.FontSize = 24;
            tempContent.FontSize = 15;
            tempTitle.Text = "YOU ARE DONE";
            tempContent.TextWrapping = TextWrapping.WrapWholeWords;
            tempContent.MaxWidth = 350;
            tempContent.Text = "enjoy your query plz";
            tempPanel.Children.Add(tempTitle);
            tempPanel.Children.Add(tempContent);
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(cur, fly);
            Flyout.ShowAttachedFlyout(cur);
        }
    }
}
