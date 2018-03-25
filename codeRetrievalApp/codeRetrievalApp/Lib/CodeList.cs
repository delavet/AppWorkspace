using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;

namespace codeRetrievalApp.Lib
{
    class CodeList : ObservableCollection<CodeInfo>, ISupportIncrementalLoading
    {
        private bool busy = false;
        private bool has_more_items = false;
        private int current_page = 0;
        public int TotalCount;
        public event DataLoadingEventHandler DataLoading;
        public event DataLoadedEventHandler DataLoaded;

        public bool HasMoreItems
        {
            get
            {
                if (busy) return false;
                else return has_more_items;
            }
            private set
            {
                has_more_items = value;
            }
        }

        public CodeList()
        {
            has_more_items = true;
        }

        public void do_fresh()
        {
            current_page = 0;
            Clear();
            HasMoreItems = true;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expected_count)
        {
            busy = true;
            var actualCount = 0;
            List<CodeInfo> temp_list = null;
            try
            {
                if (DataLoading != null)
                {
                    DataLoading();
                }
                temp_list = await execute_load_more();
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }
            if (temp_list != null && temp_list.Any())
            {
                actualCount = temp_list.Count;
                TotalCount += actualCount;
                current_page++;
                HasMoreItems = true;
                temp_list.ForEach((c) => { this.Add(c); });
            }
            else
            {
                await (new MessageDialog("加载代码失败!")).ShowAsync();
                HasMoreItems = false;
            }
            if (DataLoaded != null)
            {
                DataLoaded();
            }
            busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }

        private async Task<List<CodeInfo>> execute_load_more()
        {
            var testCode = "public class TableRowHeaderTest {		    public static void main(String[] args) {	        new TableRowHeaderTest();	    }		    public TableRowHeaderTest() {	        EventQueue.invokeLater(new Runnable() {	            @Override	            public void run() {	                try {	                    UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());	                } catch (ClassNotFoundException | InstantiationException | IllegalAccessException | UnsupportedLookAndFeelException ex)...";
            var testPost = "How can i display my Jtable to this .., , ,currently i only know to create this kind of jtable , , ,below is my code, ,HERE, , ,This is a proof of concept only, ,Disclaimer:, Before I get a bunch of hate mail about the, obviously, horrible things I've done to make this work, I stole most of the painting code straight out of the source, this is how it's actually done within the look and feel code itself :P, ,I've also gone to the nth degree, meaning that I've literally...";
            var testTitle = "How to Display Row Header on JTable Instead of Column Header";
            List<CodeInfo> more_infos = new List<CodeInfo>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://www.baidu.com"));
            for (int i = 0; i < 10; i++)
            {
                more_infos.Add(new CodeInfo(testCode, testPost, testTitle));
            }
            return more_infos;
        }
    }
}
