using System.Windows;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttClientAddTopicWindow.xaml
    /// </summary>
    public partial class MqttClientAddTopicWindow : Window
    {
        public MqttClientAddTopicWindow()
        {
            InitializeComponent();
        }

        public MqttClientAddTopicWindow(object context)
        {
            DataContext = context;

            InitializeComponent();
        }

        private void ButApply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
