using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using xLibV100.Ports;

namespace Terminal.UI
{
    public class AvailablePortForBridgeViewModel : ViewModelBase<PortBase>, IBridgeOfPortViewModel, ICellElement
    {
        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortInfoViewModel ParentInfoViewModel { get; set; }

        public AvailablePortForBridgeViewModel(PortBase model) : base(model)
        {
            Name = "Port";

            InfoViewModel = new PortInfoViewModel(model) { Parent = this };
            InfoViewModel.ViewUpdateEvent += SubViewUpdateEventHandler;

            if (model.Parent != null)
            {
                ParentInfoViewModel = new PortInfoViewModel(model.Parent) {  Parent = this };
                ParentInfoViewModel.ViewUpdateEvent += SubViewUpdateEventHandler;
            }
        }

        private void SubViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        public override void Dispose()
        {
            base.Dispose();

            InfoViewModel.Dispose();
            ParentInfoViewModel.Dispose();
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch(key)
            {
                case "Info": return InfoViewModel.Template;
                case "Parent": return ParentInfoViewModel?.Template;
                default: return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }
    }
}
