using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace codeRetrievalApp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CodeDetailPage : Page
    {
        private String html;
        private CodeInfo info;
        public CodeDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SetTitleBar(GRIDtitle);
            
            var connectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("controlAnimation");
            var connectedPostAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("postAnimation");
            if (connectedAnimation != null)
            {
                connectedAnimation.TryStart(GRIDtitle, new UIElement[] {  });
                connectedPostAnimation.TryStart(GRIDpost, new UIElement[] { });
            }
            info = e.Parameter as CodeInfo;
            if (e.Parameter != null && e.Parameter is CodeInfo)
            {
                var info = e.Parameter as CodeInfo;
                TXTBLKtitle.Text = info.title;
                WEBpreprocess();
            }

        }

        private String HandleHTML(String src)
        {
            var srcDoc = new HtmlDocument();
            srcDoc.LoadHtml(src);
            var posts = srcDoc.DocumentNode.SelectNodes("//div[@class='post-text']");
            var csss = srcDoc.DocumentNode.SelectNodes("//link[@rel='stylesheet']");
            String head = "<html>" +
                "<head>" +
                "<meta charset=\"utf-8\"/>\n" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/>" +
                "<link href=\"https://cdn.muicss.com/mui-latest/css/mui.min.css\" rel=\"stylesheet\" type=\"text/css\"/>" +
                "<script src=\"https://cdn.muicss.com/mui-latest/js/mui.min.js\"></script>" +
                "<style type=\"text/css\">" +
                "body" +
                "{" +
                "font-size:140%;" +
                "}" +
                "</style>" +
                "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.12.0/styles/default.min.css\">\n" +
                "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.12.0/highlight.min.js\"></script>" +
                "<script>hljs.initHighlightingOnLoad();</script>";
            String secHead=
                "</head>" +
                "<body>" +
                "<div id=\"content\" class=\"mui-container-fluid\">" +
                 "<div class=\"mui-divider\"></div>";
            head += secHead;
            String tail = "</div></body>" +
                "</html>";
            String content = "";
            var i = 0;
            foreach(var post in posts)
            {
                post.AddClass("mui-panel");
                content += post.OuterHtml;
                if (i == 0)
                {
                    content += "<div class=\"mui-divider\"></div>";
                    content += "<div class=\"mui--text-black mui--text-display1\">ANSWERS</div>";
                }
                i++;
            }
             String ret = head+content+tail;

            return ret;
        }
        
        private async void WEBpreprocess()
        {
            try
            {
                Parameters result = await WebConnection.ConnctWithGet("https://stackoverflow.com/questions/"+info.ID.ToString());
                if (!result.name.Equals("200")) return;
                html = HandleHTML(result.value);
                WEBpost.NavigateToString(html);
            }
            catch
            {
                return;
            }
            finally
            {
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(TXTBLKcode.Text);
            Clipboard.SetContent(dataPackage);*/
        }

        private async void WEBpost_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                /*
                string js = "";

                js += "var child=document.getElementById('sidebar');";
                js += "child.parentNode.removeChild(child);";
                js += "var banner=document.getElementsByClassName('top-bar js-top-bar _fixed');";
                js += "var i;";
                js += "for (i=0; i< banner.length; i++) { banner[i].parentNode.removeChild(banner[i]); }";
                js += "var f=document.getElementById('post-form');";
                js += "f.parentNode.removeChild(f);";
                js += "var footer=document.getElementById('footer');";
                js += "footer.parentNode.removeChild(footer);";
                await WEBpost.InvokeScriptAsync("eval", new string[] { js });
                */
            }
            catch(Exception e)
            {
                return;
            }
        }

        private void BTNsource_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTNcode_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CodeEditPage), html);
        }

        private void WEBpost_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            args.StopPageScriptExecution = true;
        }

        private async void WEBpost_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            
        }
    }
}
