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
    public sealed partial class AssociateControl : UserControl
    {
        public AssociateControl()
        {
            this.InitializeComponent();
            Flyout fly = new Flyout();
            fly.FlyoutPresenterStyle = new Style(typeof(FlyoutPresenter));
            StackPanel tempPanel = new StackPanel();
            tempPanel.Orientation = Orientation.Horizontal;
            Button button1 = new Button();
            button1.FontSize = 15;
            button1.Content = "test1";
            button1.Margin = new Thickness(3);
            button1.Click += Button_Click;
            Button button2 = new Button();
            button2.FontSize = 15;
            button2.Content = "test2";
            button2.Margin = new Thickness(3);
            button2.Click += Button_Click;
            tempPanel.Children.Add(button1);
            tempPanel.Children.Add(button2);
            fly.Content = tempPanel;
            FlyoutBase.SetAttachedFlyout(BDcore, fly);
        }

        private void BDcore_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                Flyout.ShowAttachedFlyout(element);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            String str = btn.Content as String;
            Constants.mainPage.AddInput(str);
        }
    }
}
