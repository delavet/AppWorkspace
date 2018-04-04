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
using HtmlAgilityPack;
using Windows.ApplicationModel.DataTransfer;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace codeRetrievalApp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CodeEditPage : Page
    {
        private String PageHtml;
        public CodeEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is String)) return;
            PageHtml = e.Parameter as String;
            Initialize();
        }

        private void Initialize()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(PageHtml);
            var pres = doc.DocumentNode.SelectNodes("//pre");
            int i = 0;
            foreach(var pre in pres)
            {
                i++;
                var codeText = pre.InnerText;
                Grid g = new Grid();
                TextBlock tempTXT = new TextBlock();
                tempTXT.Text = codeText;
                Button tempBTN = new Button();
                tempBTN.DataContext = codeText;
                tempBTN.Content = "Copy to Clipboard";
                tempBTN.Click += Clip_Click;
                tempBTN.VerticalAlignment = VerticalAlignment.Bottom;
                tempBTN.HorizontalAlignment = HorizontalAlignment.Center;
                ScrollViewer scr = new ScrollViewer();
                scr.HorizontalScrollMode = ScrollMode.Disabled;
                scr.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                scr.VerticalScrollMode = ScrollMode.Enabled;
                scr.Content = tempTXT;
                g.Margin = new Thickness(10, 5, 10, 0);
                g.Children.Add(scr);
                g.Children.Add(tempBTN);
                PivotItem item = new PivotItem();
                item.Header = "code" + i.ToString();
                item.Content = g;
                PVT.Items.Add(item);
            }
        }

        private void Clip_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            String code = btn.DataContext as String;
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(code);
            Clipboard.SetContent(dataPackage);
        }
    }
}
