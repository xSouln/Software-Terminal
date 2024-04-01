using System.Windows.Controls;

namespace Terminal.UI
{
    /// <summary>
    /// Логика взаимодействия для SerialPortCreatorView.xaml
    /// </summary>
    public partial class SerialPortCreatorView : UserControl
    {
        private ISerialPortCreatorViewModel ViewModel { get; set; }

        public SerialPortCreatorView()
        {
            InitializeComponent();
        }

        public SerialPortCreatorView(ISerialPortCreatorViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
