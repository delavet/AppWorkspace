using codeRetrievalApp.Lib;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using System.Text.RegularExpressions;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace codeRetrievalApp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CodeDetailPage : Page
    {
        private CodeInfo info;
        private String html;
        private String _originHtml;
        private Boolean _originMode = false;

        CompositionPropertySet _props;
        CompositionPropertySet _scrollerPropertySet;
        Compositor _compositor;

        public CodeDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.SetTitleBar(GRIDtitle);

            var connectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("postAnimation");
            if (connectedAnimation != null)
            {
                connectedAnimation.TryStart(Header, new UIElement[] { });
            }
            info = e.Parameter as CodeInfo;
            if (e.Parameter != null && e.Parameter is CodeInfo)
            {
                var info = e.Parameter as CodeInfo;
                TitleBlock.Text = info.title;
                WEBpreprocess();
            }

        }

        private String HandleHTML(String src)
        {
            _originHtml = "";
            var srcDoc = new HtmlDocument();
            srcDoc.LoadHtml(src);
            var posts = srcDoc.DocumentNode.SelectNodes("//div[@class='post-text']");
            if (posts == null) return "";
            var csss = srcDoc.DocumentNode.SelectNodes("//link[@rel='stylesheet']");

            String head = "<html>" +
                "<head>" +
                "<meta charset=\"utf-8\"/>\n" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/>" +
                "<link href=\"http://127.0.0.1:8000/static/mui.min.css\" rel=\"stylesheet\" type=\"text/css\"/>" +
                "<script src=\"http://127.0.0.1:8000/static/mui.min.js\"></script>" +
                "<style type=\"text/css\">" +
                "body" +
                "{" +
                "font-size:140%;" +
                "}" +
                "</style>" +
                "<link rel=\"stylesheet\" href=\"http://127.0.0.1:8000/static/default.min.css\">\n" +
                "<script src=\"http://127.0.0.1:8000/static/highlight.min.js\"></script>" +
                "<script>" +
                "hljs.initHighlightingOnLoad();" +
                "function myClick(e){" +
                "var dataId=e.getAttribute(\"data-id\");" +
                "window.external.notify(dataId);" +
                "}" +
                "</script>";
            String secHead =
                "</head>" +
                "<body>" +
                "<div id=\"content\" class=\"mui-container-fluid\">" +
                 "<div class=\"mui-divider\"></div>";
            head += secHead;
            String tail = "</div></body>" +
                "</html>";
            String content = "";
            var i = 0;
            var codeCounter = 0;

            foreach (var post in posts)
            {
                if (i == 0)
                {
                    string blurbText = post.InnerText;
                    blurbText = Regex.Replace(blurbText, "\\s+", " ");
                    if (blurbText.Length > 160) blurbText = blurbText.Substring(0, 160) + "...";
                    Blurb.Text = blurbText;
                    i++;
                    continue;
                    //content += "<div class=\"mui-divider\"></div>";
                    //content += "<div class=\"mui--text-black mui--text-display1\">ANSWERS</div>";
                }
                post.AddClass("mui-panel");
                var pres = post.SelectNodes(".//pre");
                if (pres != null && pres.Count > 0)
                {
                    foreach (var pre in pres)
                    {
                        pre.Attributes.Add("data-id", codeCounter.ToString());
                        pre.Attributes.Add("onclick", "myClick(this);");
                        codeCounter++;
                    }
                }
                content += post.OuterHtml;

                i++;
            }

            i = 0;
            codeCounter = 0;
            foreach (var post in posts)
            {
                post.AddClass("mui-panel");
                var pres = post.SelectNodes(".//pre");
                if (pres != null && pres.Count > 0)
                {
                    foreach (var pre in pres)
                    {
                        pre.Attributes.Add("data-id", codeCounter.ToString());
                        pre.Attributes.Add("onclick", "myClick(this);");
                        codeCounter++;
                    }
                }
                _originHtml += post.OuterHtml;
                if (i == 0)
                {
                    _originHtml += "<div class=\"mui-divider\"></div>";
                    _originHtml += "<div class=\"mui--text-black mui--text-display1\">ANSWERS</div>";
                }
                i++;
            }
            String ret = head + content + tail;
            _originHtml = head + _originHtml + tail;
            return ret;
        }

        private async void WEBpreprocess()
        {
            try
            {
                PRGRS.ProgressStart();
                HYPERsource.NavigateUri = new Uri("https://stackoverflow.com/questions/" + info.ID);
                Parameters result = await WebConnection.ConnctWithGet("https://stackoverflow.com/questions/"+info.ID);
                if (!result.name.Equals("200")) return;
                html = HandleHTML(result.value);
                WEBpost.NavigateToString(html);
                PRGRS.ProgressEnd();
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
                WEBpost.Height = Convert.ToInt32(await WEBpost.InvokeScriptAsync("eval", new[] { "document.documentElement.scrollHeight.toString();" }));
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void BTNsource_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTNcode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WEBpost_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            args.StopPageScriptExecution = true;
        }

        private async void WEBpost_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            //WEBpost.Height = Convert.ToInt32(await WEBpost.InvokeScriptAsync("eval", new [] { "document.documentElement.scrollHeight.toString();" }));
        }

        private void SCRpost_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            int a = 0;
            return;
        }

        private void WEBpost_ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                int codeId = 0;
                if (e.Value is String)
                {
                    codeId = int.Parse(e.Value);
                    var doc = new HtmlDocument();
                    if (_originMode)
                    {
                        doc.LoadHtml(_originHtml);
                    }
                    else
                    {
                        doc.LoadHtml(html);
                    }
                    var pres = doc.DocumentNode.SelectNodes("//pre");
                    var pre = pres[codeId];
                    var codeText = pre.InnerText;
                    codeText = codeText.Replace("&lt;", "<");
                    codeText = codeText.Replace("&gt;", ">");
                    CODE.OriginCode = codeText;
                    CODE.Visibility = Visibility.Visible;
                    CODE.Opacity = 1;
                    CODE.popin();
                }
            }
            catch
            {
                return;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            _scrollerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(SCRpost);
            _compositor = _scrollerPropertySet.Compositor;
            _props = _compositor.CreatePropertySet();
            _props.InsertScalar("progress", 0);
            _props.InsertScalar("clampSize", 150);
            _props.InsertScalar("scaleFactor", 0.7f);

            var progressAnimation = _compositor.CreateExpressionAnimation("Clamp(-scrollingProperties.Translation.Y/150,0,1)");
            progressAnimation.SetReferenceParameter("scrollingProperties", _scrollerPropertySet);
            _props.StartAnimation("progress", progressAnimation);
            Visual headerVisual = ElementCompositionPreview.GetElementVisual(Header);
            /*
            var headerScaleAnimation = _compositor.CreateExpressionAnimation("Lerp(1,1.25f,Clamp(scrollingProperties.Translation.Y/50,0,1))");
            headerScaleAnimation.SetReferenceParameter("scrollingProperties", _scrollerPropertySet);
            headerVisual.StartAnimation("Scale.X", headerScaleAnimation);
            headerVisual.StartAnimation("Scale.Y", headerScaleAnimation);
            headerVisual.CenterPoint= new Vector3((float)(Header.ActualWidth / 2), (float)Header.ActualHeight, 0);*/
            var headerOffsetAnimation = _compositor.CreateExpressionAnimation("-150*props.progress");
            headerOffsetAnimation.SetReferenceParameter("props", _props);
            headerVisual.StartAnimation("Offset.Y", headerOffsetAnimation);
            Visual photoVisual = ElementCompositionPreview.GetElementVisual(RECTback);
            var photoOpacityAnimation = _compositor.CreateExpressionAnimation("1-props.progress");
            photoOpacityAnimation.SetReferenceParameter("props", _props);
            photoVisual.StartAnimation("Opacity", photoOpacityAnimation);
            Visual overlayVisual = ElementCompositionPreview.GetElementVisual(OverlayRectangle);
            var overlayOpacityAnimation = _compositor.CreateExpressionAnimation("props.progress");
            overlayOpacityAnimation.SetReferenceParameter("props", _props);
            overlayVisual.StartAnimation("Opacity", overlayOpacityAnimation);
            Visual blurbVisual = ElementCompositionPreview.GetElementVisual(Blurb);
            var textOpacityAnimation = _compositor.CreateExpressionAnimation("Clamp(1-(props.progress*2),0,1)");
            textOpacityAnimation.SetReferenceParameter("props", _props);
            var scaleAnimation = _compositor.CreateExpressionAnimation("Lerp(1,0.7f,props.progress)");
            scaleAnimation.SetReferenceParameter("props", _props);
            blurbVisual.StartAnimation("Opacity", textOpacityAnimation);
            blurbVisual.StartAnimation("Scale.X", scaleAnimation);
            blurbVisual.StartAnimation("Scale.Y", scaleAnimation);
            Visual titleVisual = ElementCompositionPreview.GetElementVisual(TitleBlock);
            Visual buttonVisual = ElementCompositionPreview.GetElementVisual(ButtonPanel);
            var contentOffsetAnimation = _compositor.CreateExpressionAnimation("(props.progress)*120");
            var titleScaleAnimation = _compositor.CreateExpressionAnimation("Lerp(1,1.05f,props.progress)");
            contentOffsetAnimation.SetReferenceParameter("props", _props);
            titleScaleAnimation.SetReferenceParameter("props", _props);
            titleVisual.StartAnimation("Offset.Y", contentOffsetAnimation);
            titleVisual.StartAnimation("Scale.X", titleScaleAnimation);
            titleVisual.StartAnimation("Scale.Y", titleScaleAnimation);
            var buttonOffsetAnimation = _compositor.CreateExpressionAnimation("(props.progress*30)");
            buttonOffsetAnimation.SetReferenceParameter("props", _props);
            buttonVisual.StartAnimation("Offset.Y", buttonOffsetAnimation);
        }

        private async void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                WEBpost.Height = Convert.ToInt32(await WEBpost.InvokeScriptAsync("eval", new[] { "document.documentElement.scrollHeight.toString();" }));
            }
            catch
            {

            }
        }

        private void HYPERmore_Click(object sender, RoutedEventArgs e)
        {
            if (!_originMode)
            {
                _originMode = true;
                WEBpost.NavigateToString(_originHtml);
                HYPERmore.Content = "less";
            }
            else
            {
                _originMode = false;
                WEBpost.NavigateToString(html);
                HYPERmore.Content = "more";
            }
        }
    }
}
