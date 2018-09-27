using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace codeRetrievalApp.Lib
{
    class CodeInfo : INotifyPropertyChanged
    {
        private int id;
        private string _code;
        private string _post;
        private string _title;
        
        public CodeInfo(String c, String p, String t, int i)
        {
            id = i;
            code = c;
            post = p;
            title = t;
        }
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public String code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }
        public String post
        {
            get
            {
                return _post;
            }
            set
            {
                var srcDoc = new HtmlDocument();
                srcDoc.LoadHtml(value);
                var text = srcDoc.DocumentNode.InnerText;
                _post = text;
                OnPropertyChanged();
            }
        }
        public String title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Paragraph RichTextCode
        {
            get
            {
                Paragraph p = null;
                string xaml = "<Paragraph  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">";
                try
                {
                    var lines = code.Split('\n');
                    foreach (var line in lines)
                    {
                        var ids = Util.SplitCodeLine(line);
                        foreach (var id in ids)
                        {
                            xaml += Util.GetSpanOfId(id);
                        }
                        xaml += "<LineBreak/><Span/>";
                    }
                    xaml += "</Paragraph>";
                    p = (Paragraph)XamlReader.Load(xaml);
                }
                catch
                {
                    p = null;
                }
                return p;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
