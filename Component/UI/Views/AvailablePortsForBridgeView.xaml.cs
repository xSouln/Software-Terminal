using System.Windows;
using System.Windows.Controls;
using xLibV100.UI;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для PortBridgeView.xaml
    /// </summary>
    public partial class AvailablePortsForBridgeView : UserControl
    {
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
            set => DataContext = value;
        }

        public AvailablePortsForBridgeView()
        {
            InitializeComponent();

            DataContextChanged += DataContextChangedHandler;
        }

        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IAvailablePortsForBridgeViewModel && e.OldValue is ViewModelBase viewModel)
            {
                viewModel.UpdateEvent -= UpdateEventHandler;
            }

            if (e.NewValue is IAvailablePortsForBridgeViewModel)
            {
                ViewModel.UpdateEvent += UpdateEventHandler;
            }
        }

        private void UpdateListViewColumnsHandler()
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

        private void UpdateEventHandler(ViewModelBase viewModel)
        {
            UpdateListViewColumnsHandler();
            UpdateLayout();
        }
    }
}
