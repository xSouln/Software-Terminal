using System.Net;
using xLibV100.Controls;
using xLibV100.Net;
using xLibV100.UI;

namespace Terminal.UI
{
    public class TcpClientCreatorViewModel : ViewModelBase<Control, TcpClientCreatorView>, ITCPServerCreatorViewModel
    {
        protected string ip = "192.168.0.50";
        private Control Control { get; set; }

        public TcpClientCreatorViewModel(Control model) : base(model)
        {
            Name = "tcp client";
            Control = model;

            View = new TcpClientCreatorView(this);
            UIModel = View;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public Results Create()
        {
            var port = new TCPClient()
            {
                Name = PortName,
                Ip = Ip,
                Port = Port
            };

            Control?.AddPort(port);

            return Results.Accept;
        }

        public void Remove()
        {

        }

        public Results AddSubport()
        {
            return Results.NotSupported;
        }

        public override void Close()
        {

        }

        public void RemoveSubport()
        {

        }

        public string PortName { get; set; } = "tcp client";

        public int Port { get; set; } = 5000;

        public int RxBufferSize { get; set; } = 10000;

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
                    }

                    ip = value;
                }
                catch
                {
                    OnPropertyChanged(nameof(Ip));
                }
            }
        }
    }
}
