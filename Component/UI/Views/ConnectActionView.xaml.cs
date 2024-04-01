using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для ConnectActionView.xaml
    /// </summary>
    public partial class ConnectActionView : UserControl
    {
        public ConnectActionView()
        {
            InitializeComponent();
        }

        private void ButConnectAction_Click(object sender, RoutedEventArgs e)
        {
            IPortViewModel viewModel = DataContext as IPortViewModel;

            if (viewModel != null)
            {
                viewModel.ConnectAction();
            }
        }
    }
}
