using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xLibV100.Controls;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class BridgesOfPortsViewModel : ViewModelBase<Control, FrameworkElement>, IBridgesOfPortsViewModel
    {
        protected List<PortBase> Ports = new List<PortBase>();

        public RelayCommand RemoveBridgeCommand { get; set; }

        public BridgesOfPortsViewModel(Control model) : base(model)
        {
            Name = "Bridges";

            RemoveBridgeCommand = new RelayCommand(RemoveBridgeCommandHandler);

            model.PortAdded += PortAddedHandler;
            model.PortRemoved += PortRemovedHandler;

            AddPorts(Ports, model.AvailablePorts);
        }

        private void RemoveBridgeCommandHandler(object obj)
        {
            if (SelectedElement != null)
            {
                SelectedElement.Port.RemoveBridge(SelectedElement.Bridge);
            }
        }

        public void AddPorts(ICollection<PortBase> ports, ICollection<PortBase> collection)
        {
            foreach (var port in collection)
            {
                port.SubPortAdded += SubPortAddedHandler;
                port.SubPortRemoved += SubPortRemovedHandler;

                port.BridgeAdded += BridgeAddedHandler;
                port.BridgeRemoved += BridgeRemovedHandler;

                if (port.SubPorts != null)
                {
                    AddPorts(ports, port.SubPorts);
                }
            }
        }

        public void RemovePorts(ICollection<PortBase> ports, ICollection<PortBase> collection)
        {
            foreach (var port in collection)
            {
                if (Ports.Contains(port))
                {
                    port.SubPortAdded -= SubPortAddedHandler;
                    port.SubPortRemoved -= SubPortRemovedHandler;

                    port.BridgeAdded -= BridgeAddedHandler;
                    port.BridgeRemoved -= BridgeRemovedHandler;

                    if (port.SubPorts != null)
                    {
                        RemovePorts(ports, port.SubPorts);
                    }
                }
            }
        }

        private void SubPortRemovedHandler(PortBase port, PortBase subPort)
        {
            RemovePorts(Ports, new PortBase[] { subPort });
        }

        private void SubPortAddedHandler(PortBase port, PortBase subPort)
        {
            AddPorts(Ports, new PortBase[] { subPort });
        }

        private void BridgeRemovedHandler(PortBase port, PortBase bridgePort)
        {
            foreach (BridgeOfPortViewModel viewModel in Properties.Cast<BridgeOfPortViewModel>())
            {
                if (viewModel.Bridge == bridgePort)
                {
                    Properties.Remove(viewModel);
                    break;
                }
            }
        }

        private void BridgeAddedHandler(PortBase port, PortBase bridgePort)
        {
            var viewModel = new BridgeOfPortViewModel(port, bridgePort) { Parent = this };
            viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

            Properties.Add(viewModel);
        }

        private void SubViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        public override void Dispose()
        {
            base.Dispose();

            Model.PortAdded -= PortAddedHandler;
            Model.PortRemoved -= PortRemovedHandler;

            foreach (var port in Ports)
            {
                port.SubPortAdded -= SubPortAddedHandler;
                port.SubPortRemoved -= SubPortRemovedHandler;

                port.BridgeAdded -= BridgeAddedHandler;
                port.BridgeRemoved -= BridgeRemovedHandler;
            }
        }

        public BridgeOfPortViewModel SelectedElement { get; set; }

        private void PortAddedHandler(TerminalBase terminal, PortBase port)
        {
            var viewModel = new BridgeOfPortViewModel(port) { Parent = this };
            viewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

            Models.Add(viewModel);
        }

        private void PortRemovedHandler(TerminalBase terminal, PortBase port)
        {
            foreach(BridgeOfPortViewModel model in Models.Cast<BridgeOfPortViewModel>())
            {
                if (model.Model == port)
                {
                    Models.Remove(model);
                    break;
                }
            }
        }
    }

    public class BridgesOfPortsViewModel<TView> : BridgesOfPortsViewModel where TView : FrameworkElement, new()
    {
        public BridgesOfPortsViewModel(Control model) : base(model)
        {
            View = new TView();
            View.DataContext = this;

            var frameworkElement = new FrameworkElementFactory(typeof(TView));
            frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

            Template = new DataTemplate { VisualTree = frameworkElement };
        }
    }
}
