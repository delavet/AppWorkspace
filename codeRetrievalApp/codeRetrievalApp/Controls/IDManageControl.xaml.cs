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
using Windows.Data.Json;
using Windows.UI.Xaml.Documents;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace codeRetrievalApp.Controls
{
    public sealed partial class IDManageControl : UserControl
    {
        public String IDname
        {
            get
            {
                return TXTBLKidName.Text;
            }
            set
            {
                TXTBLKidName.Text = value;
            }
        }
        private RenameHandler rename = null;
        private GetAppearanceHandler getAppr = null;
        public IDManageControl()
        {
            this.InitializeComponent();
        }

        public IDManageControl(String id,RenameHandler r=null, GetAppearanceHandler g=null)
        {
            this.InitializeComponent();
            IDname = id;
            rename = r;
            getAppr = g;
            //LoadDescription();
        }

        public async void LoadDescription()
        {
            try
            {
                PRGRS.ProgressStart();
                if (getAppr != null)
                {
                    List<Paragraph> list = getAppr(IDname);
                    foreach(var p in list)
                    {
                        if (p != null)
                        {
                            RCHTXTappr.Blocks.Add(p);
                        }
                    }
                }
                String json = "";
                json += "{\"name\":\"";
                json += IDname;
                json += "\"}";
                var result = await WebConnection.Connect_by_json("http://127.0.0.1:8000/doc", json);
                if (!result.name.Equals("200")) return;
                var ret_json = result.value;
                JsonObject j = JsonObject.Parse(ret_json);
                bool found = j.GetNamedBoolean("found");
                if (found)
                {
                    String description = j.GetNamedString("description");
                    String url = j.GetNamedString("url");
                    HYPERdetail.NavigateUri = new Uri(url);
                    TXTBLKdescription.Text = description;
                }
                PRGRS.ProgressEnd();
            }
            catch
            {
                PRGRS.ProgressEnd();
                return;
            }
        }

        private void BTNrename_Click(object sender, RoutedEventArgs e)
        {
            bool all = false;
            if (RBTNrename.IsChecked == true)
            {
                all = true;
            }
            if (rename!=null)
            {
                rename(IDname, TXTBXrename.Text, all);
                IDname = TXTBXrename.Text;
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TXTBXrename.Text == "" || TXTBXrename.Text == null)
            {
                BTNrename.IsEnabled = false;
            }
            else
            {
                BTNrename.IsEnabled = true;
            }
        }
    }
}
