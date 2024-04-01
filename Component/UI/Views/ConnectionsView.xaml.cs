using xLibV100.UI;
using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для ConnectionsView.xaml
    /// </summary>
    public partial class ConnectionsView : UserControl
    {
        private IConnectionsViewModel ViewModel
        {
            get => DataContext as IConnectionsViewModel;
            set => DataContext = value;
        }

        public ConnectionsView()
        {
            InitializeComponent();

            DataContextChanged += DataContextChangedHandler;
        }

        private void UpdateListViewColumsSize()
        {
            var listView = ListView;

            if (listView.View is GridView gridView)
            {
                foreach (var column in gridView.Columns)
                {
                    column.Width = 0;
                    column.Width = double.NaN;
                }
            }
        }

        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel is ViewModelBase viewModel)
            {
                if (e.OldValue is ViewModelBase oldViewModel)
                {
                    oldViewModel.CellSizeChanged -= CellElemntSizeChangedHandler;
                    oldViewModel.UpdateEvent -= UpdateEventHandler;
                    oldViewModel.ViewUpdateEvent -= mViewUpdateEventHandler;

                }
                viewModel.CellSizeChanged += CellElemntSizeChangedHandler;
                viewModel.UpdateEvent += UpdateEventHandler;
                viewModel.ViewUpdateEvent += mViewUpdateEventHandler;
            }
        }

        private void mViewUpdateEventHandler(ViewModelBase viewModel)
        {
            UpdateListViewColumsSize();
            //UpdateLayout();
        }

        private void UpdateEventHandler(ViewModelBase viewModel)
        {
            UpdateListViewColumsSize();
            UpdateLayout();
        }

        private void CellElemntSizeChangedHandler(ViewModelBase viewModel, ICellElement element)
        {
            UpdateListViewColumsSize();
            UpdateLayout();
        }

        public ConnectionsView(ConnectionsViewModel viewModel) : this()
        {
            ViewModel = viewModel;
        }

        private void ButAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.Add();
        }

        private void ButRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.Remove();
        }

        private void ListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateListViewColumsSize();
        }
    }
}
