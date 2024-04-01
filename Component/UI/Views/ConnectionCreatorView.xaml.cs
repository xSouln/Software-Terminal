using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для ConnectionCreatorView.xaml
    /// </summary>
    public partial class ConnectionCreatorView : UserControl
    {
        private IConnectionCreatorViewModel ViewModel { get; set; }

        public ConnectionCreatorView()
        {
            InitializeComponent();
        }

        public ConnectionCreatorView(IConnectionCreatorViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void ButApply_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.Apply();
        }
    }
}
