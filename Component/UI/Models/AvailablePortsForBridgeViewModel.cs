using xLibV100.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xLibV100.Controls;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class AvailablePortsForBridgeViewModel : ViewModelBase<Control, FrameworkElement>, IAvailablePortsForBridgeViewModel, ICellElement
    {
        protected ViewModelBase selectedPort;
        protected List<PortBase> Servers = new List<PortBase>();
        protected Control Control { get; set; }
        public RelayCommand ApplyCommand { get; set; }

        public AvailablePortsForBridgeViewModel(Control model) : base(model)
        {
            Name = "Ports";

            Control = model;

            ApplyCommand = new RelayCommand(ApplyCommandHandler);

            model.PortAdded += PortAddedHandler;
            model.PortRemoved += PortRemovedHandler;

            foreach (var port in model.AvailablePorts)
            {
                if (port.Role != Roles.Server)
                {
                    var viewModel = new AvailablePortForBridgeViewModel(port) { Parent = this };
                    viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                    Properties.Add(viewModel);
                }
                else
                {
                    Servers.Add(port);

                    port.SubPortAdded += SubPortAddedHandler;
                    port.SubPortRemoved += SubPortRemovedHandler;

                    foreach (var subport in port.SubPorts)
                    {
                        var viewModel = new AvailablePortForBridgeViewModel(subport) { Parent = this };
                        viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                        Properties.Add(viewModel);
                    }
                }
            }
        }

        private void ApplyCommandHandler(object obj)
        {
            OnApplyEvent(SelectedPort.Model);
        }

        public ViewModelBase SelectedPort
        {
            get => selectedPort;
            set
            {
                if (selectedPort != value)
                {
                    selectedPort = value;
                    OnPropertyChanged(nameof(SelectedPort));
                    OnPropertyChanged(nameof(PortIsAvailible));
                }
            }
        }

        public bool PortIsAvailible => true;

        private void SubPortRemovedHandler(PortBase port, PortBase subPort)
        {
            Control.UIHolder.Dispatcher.Invoke(() =>
            {
                if (subPort.Role != Roles.Server)
                {
                    foreach (ViewModelBase viewModel in Properties.Cast<ViewModelBase>())
                    {
                        if (viewModel.Model == subPort)
                        {
                            Properties.Remove(viewModel);
                            break;
                        }
                    }
                }
                else
                {
                    subPort.SubPortAdded -= SubPortAddedHandler;
                    subPort.SubPortRemoved -= SubPortRemovedHandler;

                    Servers.Remove(port);
                }
            });
        }

        private void SubPortAddedHandler(PortBase port, PortBase subPort)
        {
            Control.UIHolder.Dispatcher.Invoke(() =>
            {
                if (subPort.Role != Roles.Server)
                {
                    var viewModel = new AvailablePortForBridgeViewModel(subPort) { Parent = this };
                    viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                    Properties.Add(viewModel);
                }
                else
                {
                    Servers.Add(subPort);

                    port.SubPortAdded += SubPortAddedHandler;
                    port.SubPortRemoved += SubPortRemovedHandler;

                    foreach (var subport in port.SubPorts)
                    {
                        var viewModel = new AvailablePortForBridgeViewModel(subport) { Parent = this };
                        viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                        Properties.Add(viewModel);
                    }
                }
            });
        }

        private void PortRemovedHandler(TerminalBase terminal, PortBase port)
        {
            foreach (ViewModelBase viewModel in Properties.Cast<ViewModelBase>())
            {
                if (viewModel.Model == port)
                {
                    Properties.Remove(viewModel);
                    break;
                }
            }
        }

        private void PortAddedHandler(TerminalBase terminal, PortBase port)
        {
            if (port.Role != Roles.Server)
            {
                var viewModel = new AvailablePortForBridgeViewModel(port) { Parent = this };
                viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                Properties.Add(viewModel);
            }
            else
            {
                foreach (var subport in port.SubPorts)
                {
                    var viewModel = new AvailablePortForBridgeViewModel(subport) { Parent = this };
                    viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

                    Properties.Add(viewModel);
                }
            }
        }

        private void SubViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch(key)
            {
                default: return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }
    }

    public class AvailablePortsForBridgeViewModel<TView> : AvailablePortsForBridgeViewModel where TView : FrameworkElement, new()
    {
        public AvailablePortsForBridgeViewModel(Control model) : base(model)
        {
            View = new TView();
            View.DataContext = this;

            var frameworkElement = new FrameworkElementFactory(typeof(TView));
            frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

            Template = new DataTemplate { VisualTree = frameworkElement };
        }
    }
}
