using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using xLibV100.Ports;

namespace Terminal.UI
{
    public class BridgeOfPortViewModel : ViewModelBase<PortBase>, IBridgeOfPortViewModel, ICellElement
    {
        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortInfoViewModel BridgeInfoViewModel { get; set; }
        public PortBase Port { get; protected set; }
        public PortBase Bridge { get; protected set; }

        public BridgeOfPortViewModel(PortBase model, PortBase bridgePort) : this(model)
        {
            Bridge = bridgePort;

            BridgeInfoViewModel = new PortInfoViewModel(bridgePort);
            BridgeInfoViewModel.ViewUpdateEvent += ViewUpdateEventHandler;
        }

        public BridgeOfPortViewModel(PortBase model) : base(model)
        {
            Name = "Port bridge";
            Port = model;

            InfoViewModel = new PortInfoViewModel(model);
            InfoViewModel.ViewUpdateEvent += ViewUpdateEventHandler;
        }

        private void ViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        public override void Dispose()
        {
            base.Dispose();

            InfoViewModel?.Dispose();
            BridgeInfoViewModel?.Dispose();
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch (key)
            {
                case "port info": return InfoViewModel.Template;
                case "bridge info": return BridgeInfoViewModel.Template;
                default: return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }
    }
}
