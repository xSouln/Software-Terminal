using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using System.Windows.Media;
using xLibV100.Ports;

namespace Terminal.UI
{
    public class TCPServerViewModel : ViewModelBase<PortBase, FrameworkElement>, ITCPServerViewModel, IPortViewModel, ICellElement
    {
        public DataTemplate ActionTemplate { get; protected set; }

        protected TCPServerOptionsViewModel OptionsViewModel { get; set; }
        protected PortInfoViewModel InfoViewModel { get; set; }

        public TCPServerViewModel(PortBase model) : base(model)
        {
            Name = "TCPServer";

            OptionsViewModel = new TCPServerOptionsViewModel<TcpServerOptionsView>(model) { Parent = this };
            OptionsViewModel.CellSizeChanged += SubViewModelCelltSizeChangedHandler;
            OptionsViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            OptionsViewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

            InfoViewModel = new PortInfoViewModel(model) { Parent = this };
            InfoViewModel.CellSizeChanged += SubViewModelCelltSizeChangedHandler;
            InfoViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            InfoViewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

            ActionTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(ConnectActionView)) };

            model.ConnectionStateChanged += ConnectionStateChangedHandler;

            OnPropertyChanged(nameof(ConnectionStateName));
            OnPropertyChanged(nameof(ConnectionStateBackground));
        }

        private void SubViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        private void SubViewModelUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        private void SubViewModelCelltSizeChangedHandler(ViewModelBase viewModel, ICellElement element)
        {
            OnCellSizeChanged(element);
        }

        public override void Update()
        {
            if (Parent is ViewModelBase viewModel)
            {
                viewModel.Update();
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            OptionsViewModel.Dispose();

            Model.ConnectionStateChanged -= ConnectionStateChangedHandler;
        }

        private void ConnectionStateChangedHandler(PortBase port, ConnectionStateChangedEventHandlerArg arg)
        {
            OnPropertyChanged(nameof(ConnectionStateName));
            OnPropertyChanged(nameof(ConnectionStateBackground));
        }

        public bool ConnectionIsAvailable => true;

        public string ConnectionStateName
        {
            get
            {
                switch(Model.State)
                {
                    case States.Idle: return "Start";
                    case States.Starting: return "Cancel";
                    case States.Stopping: return "Cancel";

                    default: return "Stop";
                }
            }
        }

        public Brush ConnectionStateBackground
        {
            get
            {
                switch(Model.State)
                {
                    case States.Idle: return UIProperty.RED;
                    case States.Starting: return UIProperty.YELLOW;
                    case States.Started: return UIProperty.GREEN;
                    case States.Stopping: return UIProperty.YELLOW;

                    default: return UIProperty.GREEN;
                }
            }
        }

        public override void Close()
        {
            Model.Close();
        }

        public void ConnectAction()
        {
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

        public void StartAction()
        {

        }

        public void TemplateLoaded(object sender, object component)
        {

        }
    }
}
