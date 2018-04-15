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
using codeRetrievalApp.Controls;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace codeRetrievalApp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CodeEditPage : Page
    {
        private String PageHtml;
        private List<CodeControl> CodeControlList = new List<CodeControl>();
        private List<PivotItem> PVTitems = new List<PivotItem>();
        public CodeEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is String)) return;
            PageHtml = e.Parameter as String;
            Window.Current.SetTitleBar(GRIDtitle);
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
                codeText = codeText.Replace("&lt;", "<");
                codeText = codeText.Replace("&gt;", ">");
                Grid g = new Grid();
                TextBlock tempTXT = new TextBlock();
                tempTXT.Text = codeText;                
                CodeControl tempCode = new CodeControl(codeText);
                g.Margin = new Thickness(10, 5, 10, 0);
                g.Children.Add(tempCode);
                CodeControlList.Add(tempCode);
                PivotItem item = new PivotItem();
                item.Header = "code" + i.ToString();
                item.Content = g;
                PVT.Items.Add(item);
                PVTitems.Add(item);
            }

        }

        private void PopupToast()
        {
            var t = Windows.UI.Notifications.ToastTemplateType.ToastText02;
            var content = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(t);
            XmlNodeList xml = content.GetElementsByTagName("text");
            xml[0].AppendChild(content.CreateTextNode("notice"));
            xml[1].AppendChild(content.CreateTextNode("Copied to clipboard successed! Copy anywhere as you like now."));
            Windows.UI.Notifications.ToastNotification toast = new ToastNotification(content);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void Clip_Click(object sender, RoutedEventArgs e)
        {
            CodeControl temp = CodeControlList[PVT.SelectedIndex];
            String code = temp.Code;
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(code);
            Clipboard.SetContent(dataPackage);
            PopupToast();
        }

        private void BTNvar_Click(object sender, RoutedEventArgs e)
        {
            CodeControl temp = CodeControlList[PVT.SelectedIndex];
            VARS.Show(temp.vars);
        }

        private void VARS_ChangeVar(List<Lib.Parameters> list)
        {
            PivotItem item = PVTitems[PVT.SelectedIndex];
            String ori = CodeControlList[PVT.SelectedIndex].OriginCode;
            String changed = ori;
            foreach(var i in list)
            {
                changed = changed.Replace(i.name, i.value);
            }
            CodeControl c = new CodeControl(changed);
            item.Content = c;
            CodeControlList[PVT.SelectedIndex] = c;
        }
    }
}
