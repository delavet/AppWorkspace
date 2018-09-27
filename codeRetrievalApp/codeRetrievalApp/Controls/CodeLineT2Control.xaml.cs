using codeRetrievalApp.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public delegate void VoidDelegate();
    public sealed partial class CodeLineT2Control : UserControl
    {
        public VoidDelegate DetectSelect;
        private List<String> _idSegments;
        private List<IdentifierControl> _idControls;
        private String _codeLine;
        private Boolean _selected = false;
        private CodeT2Control _attached;

        public int LineNumber
        {
            get;set;
        }

        public Rectangle leftSpace
        {
            get
            {
                RECTleft.DataContext = this;
                return RECTleft;
            }
        }
        public Rectangle rightSpace
        {
            get
            {
                RECTright.DataContext = this;
                return RECTright;
            }
        }
        public Boolean Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected)
                {
                    GRIDroot.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xf7, 0xee, 0xd6));
                }
                else
                {
                    GRIDroot.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                }
            }
        }
        public String SelectedLine
        {
            get
            {
                String ret = "";
                foreach (var control in _idControls)
                {
                    if (control.Selected)
                    {
                        ret += control.Identifier;
                    }
                }
                return ret;
            }
        }
        public String CodeLine
        {
            get
            {
                return _codeLine;
            }
            set
            {
                _codeLine = value;
                GenerateIdentifier(value);
            }
        }

        public Paragraph RichTextCode
        {
            get
            {
                Paragraph p = null;
                string xaml = "<Paragraph  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Span Foreground=\"#FFAAAAAA\">    " + LineNumber.ToString() + "    </Span>";
                try
                {
                    var ids = Util.SplitCodeLine(CodeLine);
                    foreach (var id in ids)
                    {
                        xaml += Util.GetSpanOfId(id);
                    }
                    xaml += "</Paragraph>";
                    p = (Paragraph)XamlReader.Load(xaml);
                }
                catch
                {
                    string a = xaml;
                    p = null;
                }
                return p;
            }
        }

        public CodeLineT2Control()
        {
            this.InitializeComponent();
            _idSegments = new List<string>();
            _idControls = new List<IdentifierControl>();
            GenerateIdentifier("int fuckThis = new fucking(\"this is a text\" + 0xa3 + 1234) //fuck you!");
        }

        public CodeLineT2Control(String codeLine, CodeT2Control a = null, int idx = 1)
        {
            this.InitializeComponent();
            _codeLine = codeLine;
            _idSegments = new List<string>();
            _idControls = new List<IdentifierControl>();
            _attached = a;
            GenerateIdentifier(codeLine);
            LineNumber = idx;
            TXTBLKlineId.Text = idx.ToString();
        }

        private void GenerateIdentifier(String codeline)
        {
            String content = codeline;
            String backup = codeline;
            String commentPattern = "//[^\n]*$";
            String strPattern = "\"([^\"]*)\"";
            String tokenPattern = "[^a-zA-Z0-9]";
            List<String> found = new List<string>();
            var commentFound = Regex.Matches(content, commentPattern);
            foreach (Match comment in commentFound)
            {
                found.Add(comment.Value);
                content = content.Replace(comment.Value, "");
            }
            var strFound = Regex.Matches(content, strPattern);
            foreach (Match str in strFound)
            {
                found.Add(str.Value);
                content = content.Replace(str.Value, "");
            }
            var tokenFound = Regex.Matches(content, tokenPattern);
            foreach (Match token in tokenFound)
            {
                try
                {
                    if (token.Value != "")
                    {
                        found.Add(token.Value);
                        content = content.Replace(token.Value, ",");
                    }
                }
                catch
                {
                    continue;
                }
            }
            String[] identifiers = content.Split(',');
            foreach (var id in identifiers)
            {
                if (id != "")
                {
                    found.Add(id);
                }
            }
            while (found.Count > 0)
            {
                for (int i = 0; i < found.Count; i++)
                {
                    if (backup.StartsWith(found[i]))
                    {
                        _idSegments.Add(found[i]);
                        backup = backup.Remove(0, found[i].Length);
                        found.RemoveAt(i);
                        break;
                    }
                }
            }
            AddIdentiferControl();
        }
        private void AddIdentiferControl()
        {
            foreach (String id in _idSegments)
            {
                IdentifierControl temp = new IdentifierControl(id, _idSelectDetect, _idUnselectDetect, _attached);
                PNcodeLine.Children.Add(temp);
                _idControls.Add(temp);
            }
        }


        public void WholeSelect()
        {
            Selected = true;
            foreach (var control in _idControls)
            {
                control.Selected = true;
            }
        }

        public void WholeUnselect()
        {
            Selected = false;
            foreach (var control in _idControls)
            {
                control.Selected = false;
            }
        }

        private void _idSelectDetect()
        {
            if (!Selected)
            {
                Selected = true;
                foreach(var control in _idControls)
                {
                    if (control.IsSpace)
                    {
                        control.Selected = true;
                    }
                }
            }
        }

        private void _idUnselectDetect()
        {
            foreach (var control in _idControls)
            {
                if (control.Selected && !control.IsSpace) return;
            }
            Selected = false;
            foreach(var control in _idControls)
            {
                if (control.IsSpace)
                {
                    control.Selected = false;
                }
            }
        }

        public void RequestAllReplacing(String oldId, String newId)
        {
            foreach (var control in _idControls)
            {
                control.RequestAllReplacing(oldId, newId);
            }
        }

        public void MakeLineComment()
        {
            foreach (var id in _idControls)
            {
                id.MakeComment();
            }
        }

        public bool HasId(String id)
        {
            foreach(var control in _idControls)
            {
                if (control.Identifier == id)
                    return true;
            }
            return false;
        }
    }
}
