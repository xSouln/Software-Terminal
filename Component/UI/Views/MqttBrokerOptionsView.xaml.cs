using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttBrokerOptionsView.xaml
    /// </summary>
    public partial class MqttBrokerOptionsView : UserControl
    {
        public MqttBrokerOptionsView()
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
