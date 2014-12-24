using SortFilterTest.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;


namespace SortFilterTest
{
    public sealed partial class GroupDetailPage : SortFilterTest.Common.LayoutAwarePage
    {
        public GroupDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var group = SampleDataSource.GetGroup((String)navigationParameter);

            this.DefaultViewModel["Group"] = group;

            cvm.Source = group.Items;

            this.DefaultViewModel["Items"] = cvm;
        }

        CollectionViewModel<SampleDataItem> cvm = new CollectionViewModel<SampleDataItem>();

        private void testbutton1_Click(object sender, RoutedEventArgs e)
        {
            //適当なところに配置したボタンをクリックしたらタイトルを逆順にして並べ替え
            if (this.cvm.Order == null)
            {
                this.cvm.Order = (s1, s2) => { return -(s1.Title.CompareTo(s2.Title)); };
            }
            else
            {
                this.cvm.Order = null;
            }
        }
        private void testbutton2_Click(object sender, RoutedEventArgs e)
        {
            //適当なところに配置したボタンをクリックしたらタイトル文字でフィルタ
            if (this.cvm.Filter == null)
            {
                this.cvm.Filter = (s) => s.Title.Contains("3") || s.Title.Contains("5");
            }
            else
            {
                this.cvm.Filter = null;
            }
        }


        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

    }
}
