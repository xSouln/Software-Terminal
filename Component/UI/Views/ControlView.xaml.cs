using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для ControlView.xaml
    /// </summary>
    public partial class ControlView : UserControl
    {
        private IControlViewModel ViewModel;

        public ControlView()
        {
            InitializeComponent();
        }

        public ControlView(ControlViewModel viewModle)
        {
            ViewModel = viewModle;
            DataContext = viewModle;

            InitializeComponent();
        }

        private void MenuTerminalConsole_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.OpenConsole();
        }

        private void MenuTerminalConnecntions_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.OpenConnections();
        }

        private void MenuTerminalBridges_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
