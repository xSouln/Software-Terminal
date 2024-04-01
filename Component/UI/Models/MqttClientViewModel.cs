using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using System.Windows.Media;
using xLibV100.Net.MQTT;
using xLibV100.Ports;
using xLibV100.Windows;

namespace Terminal.UI
{
    public class MqttClientViewModel :ViewModelBase<MqttClient>, IPortViewModel, ICellElement
    {
        protected string connectionStateName = "";
        protected bool connectionIsAvailable;

        public RelayCommand AddTopicCommand { get; protected set; }
        public RelayCommand ConnectCommand { get; protected set; }

        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortOptionsViewModel OptionsViewModel { get; set; }
        protected MqttTopicsViewModel<MqttTopicsView> TopicsViewModel { get; set; }

        private WindowViewPresenter TopicsViewPrecenter { get; set; }

        public MqttClientViewModel(MqttClient model) : base(model)
        {
            AddTopicCommand = new RelayCommand(AddTopicCommandHandler);
            ConnectCommand = new RelayCommand(ConnectCommandHandler);

            InfoViewModel = new PortInfoViewModel(model);
            InfoViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            OptionsViewModel = new PortOptionsViewModel(model);
            OptionsViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            OptionsViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            OptionsViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            TopicsViewModel = new MqttTopicsViewModel<MqttTopicsView>(model);

            ActionTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(MqttClientActionView)) };

            ConnectionIsAvailable = true;

            Model.ConnectionStateChanged += ConnectionChangedHandler;

            OnPropertyChanged(nameof(ConnectionStateName));
            OnPropertyChanged(nameof(ConnectionStateBackground));
            OnPropertyChanged(nameof(OptionsIsAvailable));
        }

        private void ConnectCommandHandler(object obj)
        {
            if (Model.State == States.Idle)
            {
                Model.Connect();
            }
            else
            {
                Model.Disconnect();
            }
        }

        private void AddTopicCommandHandler(object obj)
        {
            if (TopicsViewPrecenter == null)
            {
                TopicsViewPrecenter = new WindowViewPresenter(TopicsViewModel.View);
                TopicsViewPrecenter.Closed += (sender, e) =>
                {
                    TopicsViewModel?.Close();
                    TopicsViewPrecenter?.Close();
                    TopicsViewPrecenter = null;
                };

                TopicsViewPrecenter.Show();
            }
            else
            {
                TopicsViewPrecenter.Activate();
            }
        }

        private void SubViewModelUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        private void SubViewModelCelltSizeChangedhandler(ViewModelBase viewModel, ICellElement element)
        {
            OnCellSizeChanged(element);
        }

        private void SubViewModelViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        private void ConnectionChangedHandler(PortBase port, ConnectionStateChangedEventHandlerArg arg)
        {
            OnPropertyChanged(nameof(ConnectionStateBackground));
            OnPropertyChanged(nameof(OptionsIsAvailable));
            OnPropertyChanged(nameof(ConnectionStateName));
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

        public DataTemplate ActionTemplate { get; set; }

        public bool ConnectionIsAvailable
        {
            get => connectionIsAvailable;
            set
            {
                connectionIsAvailable = value;
                OnPropertyChanged(nameof(ConnectionIsAvailable));
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

        public override void Close()
        {
            TopicsViewPrecenter?.Close();
            TopicsViewPrecenter = null;
        }

        public override void Dispose()
        {
            base.Dispose();

            Model.ConnectionStateChanged -= ConnectionChangedHandler;

            Close();
        }

        public void ConnectAction()
        {
            if (Model == null)
            {
                return;
            }

            switch (Model.State)
            {
                case States.Connecting:
                    Model.Disconnect();
                    break;

                case States.Connected:
                    Model.Disconnect();
                    break;

                case States.Disconnecting:
                    Model.Disconnect();
                    break;

                case States.Idle:
                    Model.Connect();
                    break;
            }
        }

        public void StartAction()
        {

        }

        public void ShowTopics()
        {
            /*
            if (topicsViewPrecenter == null)
            {
                MqttClientTopicCreatorViewModel viewModel = new MqttClientTopicCreatorViewModel(this);
                topicsViewPrecenter = new WindowViewPresenter(new MqttClientTopicsView(viewModel));
                topicsViewPrecenter.Closed += (sender, e) =>
                {
                    topicsViewPrecenter = null;
                };

                topicsViewPrecenter.Show();
            }
            else
            {
                topicsViewPrecenter?.Activate();
            }*/
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
    }
}
