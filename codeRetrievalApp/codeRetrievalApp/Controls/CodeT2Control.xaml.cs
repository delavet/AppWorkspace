using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class CodeT2Control : UserControl
    {
        private List<CodeLineT2Control> codeLineList = new List<CodeLineT2Control>();
        private String _originCode;
        public String OriginCode
        {
            get
            {
                return _originCode;
            }
            set
            {
                _originCode = value;
                InitLines(value);
            }
        }
        public String Code
        {
            get
            {
                String ret = "";
                foreach (var control in codeLineList)
                {
                    if (control.Selected)
                    {
                        ret += control.SelectedLine + "\n";
                    }
                }
                return ret;
            }
        }
        private Boolean Selecting = false;

        public CodeT2Control()
        {
            this.InitializeComponent();
        }

        private void InitLines(String code)
        {
            STKPNcode.Children.Clear();
            var lines = code.Split('\n');
            var tempId = 0;
            foreach (var line in lines)
            {
                tempId++;
                CodeLineT2Control temp = new CodeLineT2Control(line, this, tempId);
                temp.rightSpace.PointerEntered += CodeLineControl_PointerEntered;
                temp.rightSpace.PointerPressed += STKPNcode_PointerPressed;
                temp.rightSpace.PointerReleased += STKPNcode_PointerReleased;
                temp.leftSpace.PointerEntered += CodeLineControl_PointerEntered;
                temp.leftSpace.PointerPressed += STKPNcode_PointerPressed;
                temp.leftSpace.PointerReleased += STKPNcode_PointerReleased;
                STKPNcode.Children.Add(temp);
                codeLineList.Add(temp);
            }
            int i = 0;
            bool isComment = false;
            for (i = 0; i < codeLineList.Count; i++)
            {
                if (codeLineList[i].CodeLine.Trim().StartsWith("/*")) isComment = true;
                if (isComment)
                {
                    codeLineList[i].MakeLineComment();
                }
                if (codeLineList[i].CodeLine.Trim().EndsWith("*/")) isComment = false;
            }
        }

        private void CodeLineControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Selecting)
            {
                if (sender is Rectangle)
                {
                    var rec = sender as Rectangle;
                    if (rec.DataContext is CodeLineT2Control)
                    {
                        var temp = rec.DataContext as CodeLineT2Control;
                        if (temp.Selected)
                            temp.WholeUnselect();
                        else temp.WholeSelect();
                    }
                }
            }
        }

        private void STKPNcode_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Selecting = true;
            if (sender is Rectangle)
            {
                var rec = sender as Rectangle;
                if (rec.DataContext is CodeLineT2Control)
                {
                    var temp = rec.DataContext as CodeLineT2Control;
                    if (temp.Selected)
                        temp.WholeUnselect();
                    else temp.WholeSelect();
                }
            }
        }

        private void STKPNcode_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Selecting = false;
        }

        public void popin()
        {
            STRBDpopin.Begin();
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

        private void BTNcopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(Code);
            Clipboard.SetContent(dataPackage);
            PopupToast();
        }

        private void GRIDforTapped_Tapped(object sender, TappedRoutedEventArgs e)
        {
            STRBDpopout.Begin();
            this.Visibility = Visibility.Collapsed;
        }

        public void RequestAllReplacing(String oldId, String newId)
        {
            foreach (CodeLineT2Control control in codeLineList)
            {
                control.RequestAllReplacing(oldId, newId);
            }
        }

        private void BTNselect_Click(object sender, RoutedEventArgs e)
        {
            foreach(var line in codeLineList)
            {
                line.WholeSelect();
            }
        }

        public List<Paragraph> GetAppearances(String id)
        {
            List<Paragraph> ret = new List<Paragraph>();
            foreach(var line in codeLineList)
            {
                if (line.HasId(id))
                {
                    ret.Add(line.RichTextCode);
                }
            }
            return ret;
        }
    }
}
