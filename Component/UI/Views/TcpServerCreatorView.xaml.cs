using System.Windows;
using System.Windows.Controls;
using xLibV100.UI;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для TcpServerCreatorView.xaml
    /// </summary>
    public partial class TcpServerCreatorView : UserControl, IViewBase<ITCPServerCreatorViewModel>
    {
        public TcpServerCreatorView()
        {
            InitializeComponent();
        }

        public ITCPServerCreatorViewModel ViewModel
        {
            get => DataContext as ITCPServerCreatorViewModel;
            set => DataContext = value;
        }

        public void Apply(ITCPServerCreatorViewModel context)
        {
            ViewModel = context;

            UpdateLayout();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ListView listView && listView.View is GridView gridView)
            {
                foreach (var column in gridView.Columns)
                {
                    column.Width = 0; // Сначала сбросим ширину колонки, чтобы её содержимое определило ширину
                    column.Width = double.NaN; // Установим ширину на "автоматически"
                }
            }
        }
    }
}
