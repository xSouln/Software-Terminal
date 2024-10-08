﻿using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using System.Windows.Media;
using xLibV100.Net;
using xLibV100.Ports;

namespace Terminal.UI
{
    public class TcpClientViewModel : ViewModelBase<TCPClient>, IPortViewModel, ICellElement
    {
        protected string connectionStateName = "";
        protected bool connectionIsAvailable;
        protected Visibility infoVisibility = Visibility.Collapsed;
        protected Visibility optionsVisibility = Visibility.Collapsed;

        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortOptionsViewModel OptionsViewModel { get; set; }

        public TcpClientViewModel(TCPClient model) : base(model)
        {
            Model = model;

            InfoViewModel = new PortInfoViewModel(model);
            //InfoViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            //InfoViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            InfoViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            OptionsViewModel = new PortOptionsViewModel(model);
            OptionsViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            OptionsViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            OptionsViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            ActionTemplate = new DataTemplate { VisualTree = new FrameworkElementFactory(typeof(ConnectActionView)) };

            model.ConnectionStateChanged += ConnectionChangedHandler;
            ConnectionIsAvailable = true;

            OnPropertyChanged(nameof(ConnectionStateBackground));
            OnPropertyChanged(nameof(OptionsIsAvailable));
            OnPropertyChanged(nameof(ConnectionStateName));
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

        public override void Update()
        {
            if (Parent is ViewModelBase viewModel)
            {
                viewModel.Update();
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

        public void TemplateLoaded(object sender, object component)
        {

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

        public bool ConnectionIsAvailable
        {
            get => connectionIsAvailable;
            set
            {
                connectionIsAvailable = value;
                OnPropertyChanged(nameof(ConnectionIsAvailable));
            }
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
            OnUpdateEvent();
        }

        public void ToggleOptionsVisibility()
        {
            OptionsVisibility = OptionsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            OnUpdateEvent();
        }

        public void StartAction()
        {

        }

        public DataTemplate ActionTemplate { get; protected set; }

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
