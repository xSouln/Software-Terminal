using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttBrokerCreatorView.xaml
    /// </summary>
    public partial class MqttBrokerCreatorView : UserControl
    {
        public IMqttBrokerCreatorViewModel ViewModel { get; set; }

        public MqttBrokerCreatorView()
        {
            InitializeComponent();
        }

        public MqttBrokerCreatorView(IMqttBrokerCreatorViewModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel is IMqttBrokerTopicCreatorViewModel topicCreator)
            {
                topicCreator.AddTopics();
            }
        }
    }
}
