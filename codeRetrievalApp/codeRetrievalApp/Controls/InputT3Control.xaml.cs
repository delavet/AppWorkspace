using codeRetrievalApp.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class InputT3Control : UserControl
    {
        public event SearchHandler Search;
        public event AssociateKeyWordsHandler ShowAssociates;
        private List<AssociateT2Control> KeyWordControls = new List<AssociateT2Control>();
        public InputT3Control()
        {
            this.InitializeComponent();
        }

        private void BTNsearch_Click(object sender, RoutedEventArgs e)
        {
            List<String> param = new List<string>();
            foreach(var ctrl in WPPNkw.Children)
            {
                if (!(ctrl is AssociateT2Control)) continue;
                var t = ctrl as AssociateT2Control;
                param.Add(t.KeyWord);
            }
            if (Constants.KWbox != null)
            {
                foreach (var word in Constants.KWbox.KeyWords)
                {
                    param.Add(word);
                }
                Constants.KWbox.BoxClear();
            }
            
            if (Search != null) Search(param);
        }

        private void TXTBXinput_TextChanged(object sender, TextChangedEventArgs e)
        {
            String txt = TXTBXinput.Text;
            String pattern = "[^a-zA-Z]";
            var tokensStr = Regex.Replace(txt, pattern, ",");
            String[] tokens0 = tokensStr.Split(',');
            List<String> tokens = new List<string>();
            foreach(var token in tokens0)
            {
                if (token != "")
                {
                    tokens.Add(token);
                }
            }
            List<String> tempWords = new List<string>();
            List<AssociateT2Control> reuse = new List<AssociateT2Control>();
            List<Boolean> used = new List<bool>();

            foreach(var kw in WPPNkw.Children)
            {
                if (!(kw is AssociateT2Control)) continue;
                var kwControl = kw as AssociateT2Control;
                if (tokens.Contains(kwControl.KeyWord))
                {
                    tempWords.Add(kwControl.KeyWord);
                    reuse.Add(kwControl);
                    used.Add(false);
                }
            }
            WPPNkw.Children.Clear();
            foreach (var token in tokens)
            {
                if (tempWords.Contains(token))
                {
                    int idx = tempWords.IndexOf(token);
                    if (!used[idx])
                    {
                        WPPNkw.Children.Add(reuse[idx]);
                        used[idx] = true;
                    }
                    else
                    {
                        AssociateT2Control temp = new AssociateT2Control(token);
                        temp.Margin = new Thickness(2);
                        temp.ShowAssociates += PassShowAsso;
                        KeyWordControls.Add(temp);
                        WPPNkw.Children.Add(temp);
                    }
                    
                }
                else
                {
                    AssociateT2Control temp = new AssociateT2Control(token);
                    temp.Margin = new Thickness(2);
                    temp.ShowAssociates += PassShowAsso;
                    KeyWordControls.Add(temp);
                    WPPNkw.Children.Add(temp);
                }
            }
        }

        private void PassShowAsso(FrameworkElement kwItem, string kw)
        {
            ShowAssociates(kwItem, kw);
        }
    }
}
