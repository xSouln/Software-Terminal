using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttClientCreator.xaml
    /// </summary>
    public partial class MqttClientCreatorView : UserControl
    {
        public IMqttClientCreatorViewModel ViewModel { get; set; }

        public MqttClientCreatorView()
        {
            InitializeComponent();
        }

        public MqttClientCreatorView(IMqttClientCreatorViewModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;

            InitializeComponent();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
