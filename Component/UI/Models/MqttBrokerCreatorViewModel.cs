using System.Collections.ObjectModel;
using System.Net;
using xLibV100.Controls;
using xLibV100.Net.MQTT;
using xLibV100.UI;
using xLibV100.Windows;

namespace Terminal.UI
{
    public class MqttBrokerCreatorViewModel : ViewModelBase<Control, MqttBrokerCreatorView>, IMqttBrokerCreatorViewModel
    {
        private Control Control { get; set; }
        private string ip;
        private WindowViewPresenter ViewPrecenter { get; set; }

        public class Topic
        {
            public string TopicName { get; set; }
            public string RxTopic { get; set; }
            public string TxTopic { get; set; }
        }
        public ObservableCollection<Topic> Topics { get; set; } = new ObservableCollection<Topic>();

        public MqttBrokerCreatorViewModel(Control model) : base(model)
        {
            Name = "mqtt broker";
            Control = model;

            Ip = "192.168.0.110";

            View = new MqttBrokerCreatorView(this);
            UIModel = View;
        }

        public string PortName { get; set; } = "mqtt";

        public int Port { get; set; } = 1883;

        public string Ip
        {
            get => ip;
            set
            {
                try
                {
                    if (ip != value)
                    {
                        IPAddress.Parse(value);
                        ip = value;
                        OnPropertyChanged(nameof(Ip));
                    }
                }
                catch
                {

                }
            }
        }

        public string TopicName { get; set; } = "topic 1";
        public string RxTopic { get; set; } = "/topic/1";
        public string TxTopic { get; set; } = "/topic/1";
        public object SelectedTopic { get; set; }

        public Results Create()
        {
            var port = new MqttBrokerViewModel()
            {
                Name = PortName,
                Ip = Ip,
                Port = Port
            };

            foreach (var element in Topics)
            {
                port.AddSubPort(new MqttBrokerTopic
                {
                    Name = element.TopicName,
                    RxTopicName = element.RxTopic,
                    TxTopicName = element.TxTopic
                });
            }

            Control?.AddPort(port);

            ViewPrecenter?.Close();
            ViewPrecenter = null;

            return Results.Accept;
        }

        public void Remove()
        {
            Topics.Remove((Topic)SelectedTopic);
        }

        public Results AddSubport()
        {
            if (ViewPrecenter != null)
            {
                ViewPrecenter?.Close();
                ViewPrecenter = null;
            }
            return Results.NotSupported;
        }

        public void RemoveSubport()
        {

        }

        public Results AddTopic()
        {
            Topic topic = new Topic
            {
                TopicName = TopicName,
                RxTopic = RxTopic,
                TxTopic = TxTopic
            };

            //if (!Topics.Contains(new Topic{ }))
            {
                Topics.Add(topic);
            }

            return Results.Accept;
        }

        public void RemoveTopic()
        {
            Topics?.Remove((Topic)SelectedTopic);
        }

        public Results AddTopics()
        {
            if (ViewPrecenter == null)
            {
                ViewPrecenter = new WindowViewPresenter(new MqttBrokerTopicCreatorView(this));
                ViewPrecenter.Closed += (sender, e) =>
                {
                    ViewPrecenter = null;
                };

                ViewPrecenter.Show();
            }
            else
            {
                ViewPrecenter.Activate();
            }

            return Results.Accept;
        }

        public override void Dispose()
        {
            base.Dispose();

            ViewPrecenter?.Close();
            ViewPrecenter = null;
        }

        public override void Close()
        {
            ViewPrecenter?.Close();
            ViewPrecenter = null;
        }
    }
}
