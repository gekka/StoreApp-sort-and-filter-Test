//Microsoft Limited Public Licence

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SortFilterTest.Data
{
    /// <summary>WPFでのCollectionViewSourceの代用クラス</summary>
    class CollectionViewModel<T> :  INotifyPropertyChanged
    {
        public ObservableCollection<T> Source
        {
            get { return _Source; }
            set
            {
                if (_Source != value)
                {
                    if (_Source != null)
                    {
                        _Source.CollectionChanged -= _Source_CollectionChanged;
                    }
                    _Source = value;
                    if (_Source != null)
                    {
                        _Source.CollectionChanged += _Source_CollectionChanged;
                    }
                    OnPropertyChanged("Source");
                    UpdateView();
                }
            }
        }
        private ObservableCollection<T> _Source;

        private void _Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateView();
        }

        public Func<T, T, int> Order
        {
            get
            {
                return _Order;
            }
            set
            {
                if (_Order != value)
                {
                    _Order = value;
                    OnPropertyChanged("Order");
                    UpdateView();
                }
            }
        }
        private Func<T, T, int> _Order;

        public Func<T, bool> Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                if (_Filter != value)
                {
                    _Filter = value;
                    OnPropertyChanged("Filter");
                    UpdateView();
                }
            }
        }
        private Func<T, bool> _Filter;

        /// <summary>並び替えとフィルタされた結果</summary>
        [DefaultValue(true)]
        public IEnumerable<T> View
        {
            get
            {
                return _View;
            }
        }
        private IEnumerable<T> _View;

        private void UpdateView()
        {
            IEnumerable<T> ie = this.Source;
            if (ie != null)
            {
                if (this._Filter != null)
                {
                    ie = ie.Where(_Filter);
                }
                if (this.Order != null)
                {
                    ie = ie.OrderBy<T, T>((x) => x, new Comparator<T>(this.Order));
                }
            }
            this._View = ie;
            OnPropertyChanged("View");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            var pc = PropertyChanged;
            if (pc != null)
            {
                pc(this, new PropertyChangedEventArgs(name));
            }
        }

        class Comparator<T> : IComparer<T>
        {
            public Comparator(Func<T, T, int> order)
            {
                this.order = order;
            }
            Func<T, T, int> order;

            public int Compare(T x, T y)
            {
                return order(x, y);
            }
        }
    }
}
