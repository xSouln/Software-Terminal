using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для TcpClientOptionsView.xaml
    /// </summary>
    public partial class TcpClientOptionsView : UserControl
    {
        public TcpClientOptionsView()
        {
            InitializeComponent();
        }

        private void ButShow_Click(object sender, RoutedEventArgs e)
        {
            IPortOptionsViewModel viewModel = DataContext as IPortOptionsViewModel;
            if (viewModel != null)
            {
                viewModel.ToggleOptionsVisibility();
            }
        }
    }
}
