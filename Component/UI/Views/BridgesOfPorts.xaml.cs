using xLibV100.UI;
using System.Windows;
using System.Windows.Controls;
using xLibV100.UI;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для PortBridges.xaml
    /// </summary>
    public partial class BridgesOfPortsView : UserControl
    {
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }

        public BridgesOfPortsView()
        {
            InitializeComponent();

            DataContextChanged += DataContextChangedHandler;
        }

        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                if (e.OldValue is ViewModelBase viewModel)
                {
                    viewModel.CellSizeChanged -= CellSizeChangedHandler;
                    viewModel.UpdateEvent -= UpdateEventHandler;
                }

                ViewModel.CellSizeChanged += CellSizeChangedHandler;
                ViewModel.UpdateEvent += UpdateEventHandler;
            }
        }

        private void UpdateListViewColumnSize()
        {
            if (ListView.View is GridView gridView)
            {
                foreach (var column in gridView.Columns)
                {
                    column.Width = 0;
                    column.Width = double.NaN;
                }
            }
        }

        private void UpdateEventHandler(ViewModelBase viewModel)
        {
            UpdateListViewColumnSize();
            UpdateLayout();
        }

        private void CellSizeChangedHandler(ViewModelBase viewModel, ICellElement element)
        {
            UpdateListViewColumnSize();
            UpdateLayout();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateListViewColumnSize();
        }
    }
}
