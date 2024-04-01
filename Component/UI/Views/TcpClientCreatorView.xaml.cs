using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для TcpClientCreatorView.xaml
    /// </summary>
    public partial class TcpClientCreatorView : UserControl
    {
        private ITCPServerCreatorViewModel ViewModel { get; set; }

        public TcpClientCreatorView()
        {
            InitializeComponent();
        }

        public TcpClientCreatorView(ITCPServerCreatorViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }

        private void ButApply_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
