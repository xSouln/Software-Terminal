using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using System.Windows.Media;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class SerialPortViewModel : ViewModelBase<SerialPort>, IPortViewModel, ICellElement
    {
        protected string connectionStateName = "";
        protected bool connectionIsAvailable;
        protected Visibility infoVisibility = Visibility.Collapsed;
        protected Visibility optionsVisibility = Visibility.Collapsed;

        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortOptionsViewModel OptionsViewModel { get; set; }

        public SerialPortViewModel(SerialPort model) : base(model)
        {
            Model = model;

            InfoViewModel = new PortInfoViewModel(model);
            InfoViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            InfoViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            InfoViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            OptionsViewModel = new PortOptionsViewModel(model);
            OptionsViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            OptionsViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            OptionsViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            ActionTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(ConnectActionView)) };
            //InfoTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(PortInfoView)) };
            //OptionsTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(SerialPortOptionsView)) };

            model.ConnectionStateChanged += pConnectionChanged;
            ConnectionIsAvailable = true;

            OnPropertyChanged(nameof(ConnectionStateName));
            OnPropertyChanged(nameof(ConnectionStateBackground));
        }

        public override void Dispose()
        {
            base.Dispose();

            InfoViewModel?.Dispose();
            OptionsViewModel?.Dispose();
        }

        private void SubViewModelViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        private void SubViewModelUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        private void SubViewModelCelltSizeChangedhandler(ViewModelBase viewModel, ICellElement element)
        {
            OnCellSizeChanged(element);
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch (key)
            {
                case "Info": return InfoViewModel.Template;
                case "Options": return OptionsViewModel.Template;
                case "Actions": return ActionTemplate;
                default: return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }

        private void pConnectionChanged(PortBase port, ConnectionStateChangedEventHandlerArg arg)
        {
            OnPropertyChanged(nameof(ConnectionStateName));
            OnPropertyChanged(nameof(ConnectionStateBackground));
        }

        public void ConnectAction()
        {
            switch (Model.State)
            {
                case States.Connected:
                    Model.Disconnect();
                    break;

                case States.Connecting:
                    Model.Disconnect();
                    break;

                case States.Idle:
                    Model.Connect();
                    break;
            }
        }

        public void ToggleInfoVisibility()
        {
            InfoVisibility = InfoVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            Update();
        }

        public void ToggleOptionsVisibility()
        {
            OptionsVisibility = OptionsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            Update();
        }

        public void StartAction()
        {
            return;
        }

        public bool ConnectionIsAvailable
        {
            get => connectionIsAvailable;
            set
            {
                connectionIsAvailable = value;
                OnPropertyChanged(nameof(ConnectionIsAvailable));
            }
        }

        public string ConnectionStateName
        {
            get
            {
                switch (Model.State)
                {
                    case States.Connected:
                        return "Disconnect";

                    case States.Idle:
                        return "Connect";

                    case States.Connecting:
                        return "Cancel";

                    default: return "";
                }
            }
        }

        public Brush ConnectionStateBackground
        {
            get
            {
                switch (Model.State)
                {
                    case States.Connecting: return UIProperty.YELLOW;
                    case States.Idle: return UIProperty.RED;
                    case States.Connected: return UIProperty.GREEN;
                    default: return null;
                }
            }
        }

        public DataTemplate ActionTemplate { get; protected set; }

        public DataTemplate InfoTemplate { get; protected set; }

        public DataTemplate OptionsTemplate { get; protected set; }

        public Visibility InfoVisibility
        {
            get => infoVisibility;
            set
            {
                infoVisibility = value;
                OnPropertyChanged(nameof(InfoVisibility));
            }
        }

        public Visibility OptionsVisibility
        {
            get => optionsVisibility;
            set
            {
                optionsVisibility = value;
                OnPropertyChanged(nameof(OptionsVisibility));
            }
        }

        public bool InfoIsAvailable => true;

        public bool OptionsIsAvailable
        {
            get
            {
                switch (Model.State)
                {
                    case States.Idle:
                        return true;

                    default: return false;
                }
            }
        }
    }
}
