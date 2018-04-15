using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class CodeControl : UserControl
    {
        private List<CodeLineControl> controlList = new List<CodeLineControl>();
        public String OriginCode { get; set; }
        public List<String> vars = new List<string>();
        public String Code
        {
            get
            {
                String ret = "";
                foreach(var control in controlList)
                {
                    if (control.Selected)
                    {
                        ret += control.CodeLine + "\n";
                    }
                }
                return ret;
            }
        }

        private Boolean Selecting = false;

        public CodeControl()
        {
            this.InitializeComponent();
        }

        public CodeControl(String code)
        {
            this.InitializeComponent();
            InitLines(code);
        }

        private void InitLines(String code)
        {
            OriginCode = code;
            AnalysisVar();
            var lines = code.Split('\n');
            foreach(var line in lines)
            {
                CodeLineControl temp = new CodeLineControl(line);
                temp.Margin = new Thickness(10,1,10,1);
                temp.PointerEntered += CodeLineControl_PointerEntered;
                temp.PointerPressed += STKPNcode_PointerPressed;
                temp.PointerReleased += STKPNcode_PointerReleased;
                STKPNcode.Children.Add(temp);
                controlList.Add(temp);
            }
        }

        private void AnalysisVar()
        {
            String pattern = "[^a-zA-Z]";
            var tokensStr = Regex.Replace(OriginCode, pattern, ",");
            String[] tokens0 = tokensStr.Split(',');
            List<String> tokens = new List<string>();
            foreach(var t in tokens0)
            {
                if (t.Length > 2)
                {
                    tokens.Add(t);
                }
            }
            Dictionary<String, int> frequency = new Dictionary<string, int>();
            foreach(var token in tokens)
            {
                if (frequency.ContainsKey(token))
                {
                    frequency[token] = frequency[token] + 1;
                }
                else
                {
                    frequency.Add(token, 0);
                }
            }
            foreach(var token in tokens)
            {
                if (frequency[token] >= 2&&!vars.Contains(token))
                {
                    vars.Add(token);
                }
            }
        }

        private void CodeLineControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Selecting)
            {
                if(sender is CodeLineControl)
                {
                    var temp = sender as CodeLineControl;
                    temp.Selected = !temp.Selected;
                }
            }
        }

        private void STKPNcode_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Selecting = true;
            if (sender is CodeLineControl)
            {
                var temp = sender as CodeLineControl;
                temp.Selected = !temp.Selected;
            }
        }

        private void STKPNcode_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Selecting = false;
        }
    }
}
