using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttBrokerActionView.xaml
    /// </summary>
    public partial class MqttBrokerActionView : UserControl
    {
        public MqttBrokerActionView()
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

        private void ButAddTopic_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IMqttBrokerTopicCreatorViewModel action)
            {
                action.AddTopics();
            }
        }
    }
}
