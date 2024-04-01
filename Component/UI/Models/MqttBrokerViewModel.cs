using xLibV100.UI;
using System.Windows;
using System.Windows.Media;
using xLibV100.Controls;
using xLibV100.Net.MQTT;
using xLibV100.Ports;
using xLibV100.UI;
using xLibV100.Windows;

namespace Terminal.UI
{
    public class MqttBrokerViewModel : MqttBroker, IPortViewModel, IMqttBrokerTopicCreatorViewModel, ICloseableViewModel, ICellElement
    {
        protected string connectionStateName = "";
        protected bool connectionIsAvailable;
        protected Visibility infoVisibility = Visibility.Collapsed;
        protected Visibility optionsVisibility = Visibility.Collapsed;
        private WindowViewPresenter optionsViewPrecenter { get; set; }

        public PortBase Model { get; set; }

        public MqttBrokerViewModel()
        {
            Model = this;

            ActionTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(MqttBrokerActionView)) };
            //InfoTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(PortInfoView)) };
            OptionsTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(MqttBrokerOptionsView)) };

            ConnectionStateChanged += pConnectionChanged;
            ConnectionIsAvailable = true;

            OnConnectionChanged();
        }

        public MqttBrokerViewModel(PortBase port) : this()
        {
            Apply(port);
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch(key)
            {
                case "Info": return InfoTemplate;
                case "Options": return OptionsTemplate;
                case "Actions": return ActionTemplate;
                default : return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }

        private void pConnectionChanged(PortBase port, ConnectionStateChangedEventHandlerArg arg)
        {
            switch (arg.State)
            {
                case States.Starting:
                    ConnectionStateName = "Cancel";
                    break;

                case States.Idle:
                    ConnectionStateName = "Start";
                    break;

                case States.Started:
                    ConnectionStateName = "Stop";
                    break;
            }

            OnPropertyChanged(nameof(ConnectionStateBackground));
            OnPropertyChanged(nameof(OptionsIsAvailable));
        }

        public void ConnectAction()
        {
            if (Model == null)
            {
                return;
            }

            switch (Model.State)
            {
                case States.Starting:
                    Model.Stop();
                    break;

                case States.Started:
                    Model.Stop();
                    break;

                case States.Stopping:
                    Model.Stop();
                    break;

                case States.Idle:
                    Model.Start();
                    break;
            }
        }

        public void StartAction()
        {

        }

        public Results AddTopics()
        {
            if (optionsViewPrecenter == null)
            {
                optionsViewPrecenter = new WindowViewPresenter(new MqttBrokerTopicCreatorView(this));
                optionsViewPrecenter.Closed += (sender, e) =>
                {
                    optionsViewPrecenter = null;
                };

                optionsViewPrecenter.Show();
            }
            else
            {
                optionsViewPrecenter.Activate();
            }

            return Results.Accept;
        }

        public void RemoveTopic()
        {
            if (SelectedTopic != null && SelectedTopic is PortBase port)
            {
                RemoveSubPort(port);
            }
        }

        public Results AddTopic()
        {
            AddSubPort(new MqttBrokerTopic
            {
                Name = TopicName,
                TxTopicName = TxTopic,
                RxTopicName = RxTopic
            });
            return Results.Accept;
        }

        public override PortBase AddListener(TerminalObject device)
        {
            return SelectedTopic != null && SelectedTopic is PortBase port ? port.AddListener(device) : null;
        }

        public override PortBase RemoveListener(TerminalObject device)
        {
            if (SelectedTopic is PortBase port)
            {
                port.RemoveListener(device);
                return port;
            }
            return null;
        }

        public override void RemoveSubPort(PortBase port)
        {
            if (SelectedTopic is PortBase selectedTopic)
            {
                base.RemoveSubPort(selectedTopic);
            }
        }

        public override void Close()
        {
            optionsViewPrecenter?.Close();
            optionsViewPrecenter = null;
        }

        public override void Dispose()
        {
            base.Dispose();

            Close();
        }

        public bool OptionsIsAvailable
        {
            get
            {
                switch (State)
                {
                    case States.Idle:
                        return true;

                    default: return false;
                }
            }
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
            get => connectionStateName;
            set
            {
                connectionStateName = value;
                OnPropertyChanged(nameof(ConnectionStateName));
            }
        }

        public Brush ConnectionStateBackground
        {
            get
            {
                switch (State)
                {
                    case States.Starting: return UIProperty.YELLOW;
                    case States.Idle: return UIProperty.RED;
                    case States.Started: return UIProperty.GREEN;
                    default: return null;
                }
            }
        }

        public Visibility InfoVisibility
        {
            get => infoVisibility;
            protected set
            {
                infoVisibility = value;
                OnPropertyChanged(nameof(InfoVisibility));
            }
        }

        public Visibility OptionsVisibility
        {
            get => optionsVisibility;
            protected set
            {
                optionsVisibility = value;
                OnPropertyChanged(nameof(OptionsVisibility));
            }
        }

        public void ToggleInfoVisibility()
        {
            InfoVisibility = InfoVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public void ToggleOptionsVisibility()
        {
            OptionsVisibility = OptionsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public bool InfoIsAvailable => true;

        public DataTemplate ActionTemplate { get; protected set; }

        public DataTemplate InfoTemplate { get; protected set; }

        public DataTemplate OptionsTemplate { get; protected set; }
        public object SelectedTopic { get; set; }
        public string TopicName { get; set; } = "topic1";
        public string RxTopic { get; set; } = "topic/1";
        public string TxTopic { get; set; } = "topic/1";
    }
}
