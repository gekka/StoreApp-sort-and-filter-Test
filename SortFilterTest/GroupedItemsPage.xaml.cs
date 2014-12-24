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
    /// <summary>
    /// グループ化されたアイテムのコレクションを表示するページです。
    /// </summary>
    public sealed partial class GroupedItemsPage : SortFilterTest.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            ObservableCollection<SampleDataGroup> sampleDataGroups
                = SampleDataSource.GetGroups((String)navigationParameter)
                as ObservableCollection<SampleDataGroup>;
            CollectionViewSource vs = new CollectionViewSource();
            //ObservableCollectionをソースにして並べ替えやフィルタ処理できるようにする
            cvm.Source = sampleDataGroups;

            this.DefaultViewModel["Groups"] = cvm;//並び替え・フィルタ処理される結果をバインドさせるため
        }


        CollectionViewModel<SampleDataGroup> cvm = new CollectionViewModel<SampleDataGroup>();

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

        private void testbutton3_Click(object sender, RoutedEventArgs e)
        {
            //適当なところに配置したボタンをクリックしたらタ追加
            var now = DateTime.Now.ToString("mm:ss.fff");
            SampleDataGroup group = new SampleDataGroup(now, "Title"+now, "Sub", "", "description");
            cvm.Source.Add(group);
        }

        void Header_Click(object sender, RoutedEventArgs e)
        {
            var group = (sender as FrameworkElement).DataContext;
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }


    }
}
