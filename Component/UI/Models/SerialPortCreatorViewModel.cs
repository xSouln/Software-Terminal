using System.Collections.Generic;
using System.Collections.ObjectModel;
using xLibV100.Controls;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class SerialPortCreatorViewModel : ViewModelBase<Control, SerialPortCreatorView>, ISerialPortCreatorViewModel
    {
        public ObservableCollection<string> PortList { get; set; }
        public List<int> BaudRateList { get; set; }
        public Control Control { get; set; }

        public SerialPortCreatorViewModel(Control model) : base(model)
        {
            Name = "serial ports";
            Control = model;

            PortList = SerialPort.PortList;
            BaudRateList = SerialPort.BaudRateList;

            View = new SerialPortCreatorView(this);
            UIModel = View;
        }

        public string PortName { get; set; } = "";
        public int BaudRate { get; set; } = 115200;

        public Results Create()
        {
            var port = new SerialPort()
            {
                Name = PortName,
                BaudRate = BaudRate,
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
    }
}
