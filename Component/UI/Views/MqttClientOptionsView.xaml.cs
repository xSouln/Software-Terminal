using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttClientOptionsView.xaml
    /// </summary>
    public partial class MqttClientOptionsView : UserControl
    {
        public MqttClientOptionsView()
        {
            InitializeComponent();
        }

        private void ButOpenTopis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButShow_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is IPortOptionsViewModel viewModel)
            {
                viewModel.ToggleOptionsVisibility();
            }
        }
    }
}
