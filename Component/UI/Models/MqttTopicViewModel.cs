using xLibV100.Common.UI;
using xLibV100.UI;
using System.Windows;
using xLibV100.Net.MQTT;
using xLibV100.UI;

namespace Terminal.UI
{
    public class MqttTopicViewModel : ViewModelBase<MqttTopic, FrameworkElement>, ICellElement
    {
        protected PortInfoViewModel InfoViewModel { get; set; }
        protected PortOptionsViewModel OptionsViewModel { get; set; }

        public MqttTopicViewModel(MqttTopic model) : base(model)
        {
            InfoViewModel = new PortInfoViewModel(model);
            //InfoViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            //InfoViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            InfoViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

            OptionsViewModel = new PortOptionsViewModel(model);
            OptionsViewModel.CellSizeChanged += SubViewModelCelltSizeChangedhandler;
            OptionsViewModel.UpdateEvent += SubViewModelUpdateEventHandler;
            OptionsViewModel.ViewUpdateEvent += SubViewModelViewUpdateEventHandler;

        }

        private void SubViewModelUpdateEventHandler(ViewModelBase viewModel)
        {
            OnUpdateEvent();
        }

        private void SubViewModelCelltSizeChangedhandler(ViewModelBase viewModel, ICellElement element)
        {
            OnCellSizeChanged(element);
        }

        private void SubViewModelViewUpdateEventHandler(ViewModelBase viewModel)
        {
            OnViewUpdateEvent();
        }

        public DataTemplate GetTemplate(object sender, string key)
        {
            switch (key)
            {
                case "Info": return InfoViewModel.Template;
                case "Options": return OptionsViewModel.Template;
                //case "Actions": return ActionTemplate;
                default: return null;
            }
        }

        public void TemplateLoaded(object sender, object component)
        {

        }

        public class DeviceViewModel<TView> : MqttTopicViewModel
            where TView : FrameworkElement, new()
        {
            public DeviceViewModel(MqttTopic model) : base(model)
            {
                View = new TView
                {
                    DataContext = this
                };

                var frameworkElement = new FrameworkElementFactory(typeof(TView));
                frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

                Template = new DataTemplate
                {
                    VisualTree = frameworkElement
                };
            }
        }
    }
}
