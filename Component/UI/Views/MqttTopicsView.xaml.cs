using System.Windows;
using System.Windows.Controls;
using xLibV100.UI;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для MqttClientTopicsView.xaml
    /// </summary>
    public partial class MqttTopicsView : UserControl
    {
        public ViewModelBase ViewModel
        {
            get => DataContext as ViewModelBase;
        }

        public MqttTopicsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                Button button = (Button)sender;
                ViewModel.PropertyChangedRequestListener(sender, button.DataContext, button.Content, null);
            }
        }
    }
}
