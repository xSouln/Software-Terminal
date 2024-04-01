using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttClientActionView.xaml
    /// </summary>
    public partial class MqttClientActionView : UserControl
    {
        public MqttClientActionView()
        {
            InitializeComponent();
        }

        private void ButConnectAction_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IPortViewModel viewModel)
            {
                viewModel.ConnectAction();
            }
        }

        private void ButAddTopic_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IMqttClientViewModel viewModel)
            {
                viewModel.ShowTopics();
            }
        }
    }
}
