using System.Net;
using xLibV100.Controls;
using xLibV100.Net.MQTT;
using xLibV100.UI;
using xLibV100.Windows;

namespace Terminal.UI
{
    public class MqttClientCreatorViewModel : ViewModelBase<Control, MqttClientCreatorView>, IMqttClientCreatorViewModel
    {
        private string ip;

        public RelayCommand AddTopicCommand { get; protected set; }

        private WindowViewPresenter ViewPrecenter { get; set; }
        private Control Control { get; set; }

        public MqttClientCreatorViewModel(Control model) : base(model)
        {
            Name = "mqtt client";
            Control = model;

            Ip = "192.168.0.110";

            AddTopicCommand = new RelayCommand(AddTopicCommandHandler);

            View = new MqttClientCreatorView(this);
            UIModel = View;
        }

        private void AddTopicCommandHandler(object obj)
        {

        }

        public string PortName { get; set; } = "mqtt client";

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

        public override void Dispose()
        {
            base.Dispose();

            ViewPrecenter?.Close();
            ViewPrecenter = null;
        }

        public Results Create()
        {

            var port = new MqttClient()
            {
                Name = PortName,
                Address = Ip,
                Port = Port
            };

            Control?.AddPort(port);

            ViewPrecenter?.Close();
            ViewPrecenter = null;

            return Results.Accept;
        }

        public Results AddSubport()
        {
            return Results.NotSupported;
        }

        public void RemoveSubport()
        {

        }

        public void Remove()
        {

        }

        public override void Close()
        {

        }
    }
}
