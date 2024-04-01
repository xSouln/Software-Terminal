using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttBrokerTopicCreatorView.xaml
    /// </summary>
    public partial class MqttBrokerTopicCreatorView : UserControl
    {
        private IMqttBrokerTopicCreatorViewModel ViewModel { get; set; }
        public MqttBrokerTopicCreatorView()
        {
            InitializeComponent();
        }

        public MqttBrokerTopicCreatorView(IMqttBrokerTopicCreatorViewModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.AddTopic();
        }

        private void ButDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.RemoveTopic();
        }
    }
}
