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
using codeRetrievalApp.Lib;
using Windows.Data.Json;

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

        private String q = "";

        public CodeList()
        {
            has_more_items = true;
        }

        public CodeList(String query)
        {
            has_more_items = true;
            q = query;
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
            List<CodeInfo> more_infos = new List<CodeInfo>();
            String json = "";
            json += "{\"query\":\"";
            json += q;
            json += "\",\"page\":";
            json += (current_page + 1).ToString();
            json += "}";
            var result = await WebConnection.Connect_by_json("http://127.0.0.1:8000/search", json);
            if (!result.name.Equals("200")) return more_infos;
            var ret_json = result.value;
            try
            {
                JsonObject jsonObject = JsonObject.Parse(ret_json);
                String message = jsonObject.GetNamedString("message");
                if (!message.Equals("success")) return more_infos;
                JsonArray array = jsonObject.GetNamedArray("result");
                foreach(var info in array)
                {
                    JsonObject obj = info.GetObject();
                    String id = obj.GetNamedString("id");
                    String title = obj.GetNamedString("title");
                    String post = obj.GetNamedString("post");
                    String code = obj.GetNamedString("code");
                    if (title.Length > 50)
                        title = title.Substring(0, 50) + "...";
                    if (post.Length > 500)
                        post = post.Substring(0, 500) + "...";
                    if (code.Length > 500)
                        code = code.Substring(0, 500) + "...";
                    CodeInfo temp = new CodeInfo(code, post, title, int.Parse(id));
                    more_infos.Add(temp);
                }
            }
            catch
            {
                return more_infos;
            }
            return more_infos;
        }
    }
}
