using System.Windows;
using xLibV100.Net.MQTT;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class MqttTopicsViewModel : ViewModelBase<MqttClient, FrameworkElement>
    {
        public object SelectedTopic { get; set; }

        public string PortName { get; set; } = "topic 1";
        public string TopicName { get; set; } = "topic/";

        public RelayCommand AddTopicCommand { get; protected set; }
        public RelayCommand RemoveTopicCommand { get; protected set; }
        public RelayCommand IsWritableClickCommand { get; protected set; }
        public RelayCommand IsSubscribedClickCommand { get; protected set; }

        public MqttTopicsViewModel(MqttClient model) : base(model)
        {
            Name = "mqtt topic creator";

            AddTopicCommand = new RelayCommand(AddTopicCommandHandler);
            RemoveTopicCommand = new RelayCommand(RemoveTopicCommandHandler);
            IsWritableClickCommand = new RelayCommand(IsWritableClickCommandHandler);
            IsSubscribedClickCommand = new RelayCommand(IsSubscribedClickCommandHandler);
        }

        private void IsSubscribedClickCommandHandler(object obj)
        {
            if (obj is MqttTopic topic)
            {
                topic.IsSubscribed ^= true;
            }
        }

        private void IsWritableClickCommandHandler(object obj)
        {
            if (obj is MqttTopic topic)
            {
                topic.IsWriteable ^= true;
            }
        }

        private void RemoveTopicCommandHandler(object obj)
        {
            if (SelectedTopic is PortBase port)
            {
                Model.RemoveSubPort(port);
            }
        }

        public override void PropertyChangedRequestListener(object sender, object dataContext, object value, object description)
        {

        }

        private void AddTopicCommandHandler(object obj)
        {
            MqttClientAddTopicWindow addTopicWindow = new MqttClientAddTopicWindow(this);

            if (addTopicWindow.ShowDialog() == true)
            {
                Model.AddSubPort(new MqttTopic
                {
                    Name = PortName,
                    TopicName = TopicName
                });
            }
        }
    }

    public class MqttTopicsViewModel<TView> : MqttTopicsViewModel
        where TView : FrameworkElement, new()
    {
        public MqttTopicsViewModel(MqttClient model) : base(model)
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
