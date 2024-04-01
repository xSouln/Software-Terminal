using xLibV100.UI;
using xLibV100.Windows;
using xLibV100.Ports;
using xLibV100.Net;
using xLibV100.Controls;
using System.Windows;
using System.Linq;
using xLibV100.Net.MQTT;
using xLibV100.UI;

namespace Terminal.UI
{
    public class ConnectionsViewModel : ViewModelBase<Control, ConnectionsView>, IConnectionsViewModel
    {
        protected ConnectionCreatorViewModel ConnectionCreatorViewModel;
        protected WindowViewPresenter AvailablePortsForBridgeViewPrecenter { get; set; }
        protected WindowViewPresenter ViewPrecenter { get; set; }
        protected WindowViewPresenter ConnectionCreatorViewPrecenter { get; set; }
        protected AvailablePortsForBridgeViewModel AvailablePortsForBridge { get; set; }
        protected Control Control { get; set; }
        public RelayCommand AddBridgeCommand { get; protected set; }

        public ConnectionsViewModel(Control model) : base(model)
        {
            Name = model.Name;
            Control = model;

            model.PortAdded += PortAddedHandler;
            model.PortRemoved += PortRemovedHandler;

            AddBridgeCommand = new RelayCommand(AddBridgeCommandHandler);

            AvailablePortsForBridge = new AvailablePortsForBridgeViewModel<AvailablePortsForBridgeView>(model);
            AvailablePortsForBridge.ApplyEvent += AvailablePortsForBridgeApplyEventHandler;

            ConnectionCreatorViewModel = new ConnectionCreatorViewModel(model);
            ConnectionCreatorViewModel.NewCreationEvent += NewCreationEvent;

            ViewModelBase viewModelBase;

            foreach (var port in model.AvailablePorts)
            {
                switch (port)
                {
                    case SerialPort serial:
                        viewModelBase = new SerialPortViewModel(serial);
                        break;

                    case TCPClient tcpClient:
                        viewModelBase = new TcpClientViewModel(tcpClient);
                        break;

                    case TCPServer tcpServer:
                        port.SubPortAdded += SubPortAddedHandler;
                        port.SubPortRemoved += SubPortRemovedHandler;
                        viewModelBase = new TCPServerViewModel(tcpServer);
                        break;

                    case MqttClient mqttClient:
                        port.SubPortAdded += SubPortAddedHandler;
                        port.SubPortRemoved += SubPortRemovedHandler;
                        viewModelBase = new MqttClientViewModel(mqttClient);
                        break;

                    default: continue;
                }

                viewModelBase.Parent = this;
                viewModelBase.CellSizeChanged += SubmodelCellElemntSizeChangedHandler;
                viewModelBase.UpdateEvent += SubmodelUpdateEventHandler;
                viewModelBase.ViewUpdateEvent += SubViewUpdateEventHandler;
                Properties.Add(viewModelBase);

                foreach (var SubPort in port.SubPorts)
                {
                    switch (SubPort)
                    {
                        case MqttTopic mqttTopic:
                        viewModelBase = new MqttTopicViewModel(mqttTopic);
                        break;

                        default : continue;
                    }

                    viewModelBase.Parent = this;
                    viewModelBase.CellSizeChanged += SubmodelCellElemntSizeChangedHandler;
                    viewModelBase.UpdateEvent += SubmodelUpdateEventHandler;
                    viewModelBase.ViewUpdateEvent += SubViewUpdateEventHandler;
                    Properties.Add(viewModelBase);
                }
            }

            View = new ConnectionsView();
            View.DataContext = this;

            var frameworkElement = new FrameworkElementFactory(typeof(ConnectionsView));
            frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

            Template = new DataTemplate { VisualTree = frameworkElement };

            model.UI.ConnectionsViewModel = this;
        }

        private void AvailablePortsForBridgeApplyEventHandler(ViewModelBase viewModel, object result)
        {
            AvailablePortsForBridgeViewPrecenter?.Close();
            AvailablePortsForBridgeViewPrecenter = null;

            if (SelectedValue != null
                && SelectedValue.Model is PortBase port
                && result is PortBase bridge
                && port != bridge)
            {
                port.AddBridge(bridge);
            }
        }

        private void AddBridgeCommandHandler(object obj)
        {
            if (AvailablePortsForBridgeViewPrecenter == null)
            {
                AvailablePortsForBridgeViewPrecenter = new WindowViewPresenter(AvailablePortsForBridge.View);
                AvailablePortsForBridgeViewPrecenter.Closed += (sender, e) =>
                {
                    AvailablePortsForBridgeViewPrecenter = null;
                };

                AvailablePortsForBridgeViewPrecenter.Show();
            }
            else
            {
                AvailablePortsForBridgeViewPrecenter.Activate();
            }
        }

        private void SubViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
            View?.UpdateLayout();
        }

        private void SubmodelUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        public override ViewModelBase Clone()
        {
            return new ConnectionsViewModel(Model);
        }

        private void SubmodelCellElemntSizeChangedHandler(ViewModelBase viewModel, ICellElement element)
        {
            OnCellSizeChanged(element);
        }

        public override void Update()
        {
            OnUpdateEvent();
        }

        private void PortRemovedHandler(TerminalBase terminal, PortBase port)
        {
            foreach (ViewModelBase element in Properties.Cast<ViewModelBase>())
            {
                if (element.Model == port)
                {
                    Properties.Remove(element);
                    break;
                }
            }
        }

        private void PortAddedHandler(TerminalBase terminal, PortBase port)
        {
            ViewModelBase viewModelBase;

            switch (port)
            {
                case SerialPort serial:
                    viewModelBase = new SerialPortViewModel(serial);
                    break;

                case TCPClient tcpClient:
                    viewModelBase = new TcpClientViewModel(tcpClient);
                    break;

                case TCPServer tcpServer:
                    port.SubPortAdded += SubPortAddedHandler;
                    port.SubPortRemoved += SubPortRemovedHandler;
                    viewModelBase = new TCPServerViewModel(tcpServer);
                    break;

                case MqttClient mqttClient:
                    port.SubPortAdded += SubPortAddedHandler;
                    port.SubPortRemoved += SubPortRemovedHandler;
                    viewModelBase = new MqttClientViewModel(mqttClient);
                    break;

                default: return;
            }

            viewModelBase.Parent = this;
            viewModelBase.CellSizeChanged += SubmodelCellElemntSizeChangedHandler;
            viewModelBase.UpdateEvent += SubmodelUpdateEventHandler;
            viewModelBase.ViewUpdateEvent += SubViewUpdateEventHandler;
            Properties.Add(viewModelBase);
        }

        private void SubPortRemovedHandler(PortBase port, PortBase subPort)
        {
            Control.UIHolder.Dispatcher.Invoke(() =>
            {
                foreach (ViewModelBase element in Properties)
                {
                    if (element.Model == subPort)
                    {
                        Properties.Remove(element);
                        break;
                    }
                }
            });
        }

        private void SubPortAddedHandler(PortBase port, PortBase subPort)
        {
            Control.UIHolder.Dispatcher.Invoke(() =>
            {
                ViewModelBase viewModelBase;

                switch (subPort)
                {
                    case SerialPort serial:
                        viewModelBase = new SerialPortViewModel(serial);
                        break;

                    case TCPClient tcpClient:
                        viewModelBase = new TcpClientViewModel(tcpClient);
                        break;

                    case TCPServer tcpServer:
                        port.SubPortAdded += SubPortAddedHandler;
                        port.SubPortRemoved += SubPortRemovedHandler;
                        viewModelBase = new TCPServerViewModel(tcpServer);
                        break;

                    case MqttTopic mqttTopic:
                        viewModelBase = new MqttTopicViewModel(mqttTopic);
                        break;

                    default: return;
                }

                viewModelBase.Parent = this;
                viewModelBase.CellSizeChanged += SubmodelCellElemntSizeChangedHandler;
                viewModelBase.UpdateEvent += SubmodelUpdateEventHandler;
                viewModelBase.ViewUpdateEvent += SubViewUpdateEventHandler;
                Properties.Add(viewModelBase);
            });
        }

        private void NewCreationEvent(object arg, IConnectionCreatorViewModels viewModel)
        {
            ConnectionCreatorViewPrecenter?.Close();
        }

        public override void Dispose()
        {
            base.Dispose();

            ConnectionCreatorViewModel?.Dispose();

            ViewPrecenter?.Close();
            ViewPrecenter = null;

            ConnectionCreatorViewPrecenter?.Close();
            ConnectionCreatorViewPrecenter = null;
        }

        public void Open()
        {
            if (ViewPrecenter == null)
            {
                ViewPrecenter = new WindowViewPresenter(View);
                ViewPrecenter.Closed += (sender, e) =>
                {
                    ViewPrecenter = null;
                    ConnectionCreatorViewPrecenter?.Close();
                    ConnectionCreatorViewModel?.Close();
                    AvailablePortsForBridgeViewPrecenter?.Close();

                    foreach (var element in Model.AvailablePorts)
                    {
                        if (element is ICloseableViewModel viewModel)
                        {
                            viewModel.Close();
                        }
                    }
                };

                ViewPrecenter.Show();
            }
            else
            {
                ViewPrecenter.Activate();
            }
        }

        public void Add()
        {
            if (ConnectionCreatorViewPrecenter == null)
            {
                ConnectionCreatorViewPrecenter = new WindowViewPresenter(ConnectionCreatorViewModel.UIModel);
                ConnectionCreatorViewPrecenter.Closed += (sender, e) =>
                {
                    ConnectionCreatorViewPrecenter = null;
                    ConnectionCreatorViewModel?.Close();
                };

                ConnectionCreatorViewPrecenter.Show();
            }
            else
            {
                ConnectionCreatorViewPrecenter.Activate();
            }
        }

        public ViewModelBase SelectedValue { get; set; }

        public void Remove()
        {
            if (SelectedValue != null && SelectedValue.GetModel() is PortBase port)
            {
                Model?.RemovePort(port);
            }
        }

        public override void Close()
        {
            if (ConnectionCreatorViewPrecenter != null)
            {
                ConnectionCreatorViewPrecenter.Close();
                ConnectionCreatorViewPrecenter = null;
            }
        }
    }
}
