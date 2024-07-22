using xLibV100.UI;
using xLibV100.UI.CellElements;
using System.Net;
using System.Reflection;
using System.Windows;
using xLibV100.Controls;
using xLibV100.Net;

namespace Terminal.UI
{
    public class TCPServerCreatorViewModel : ViewModelBase<Control, FrameworkElement>, ITCPServerCreatorViewModel
    {
        private string _ip = "192.168.100.10";

        [UIProperty]
        public string PortName { get; set; } = "tcp server";

        [UIProperty]
        public string Ip
        {
            get => _ip;
            set
            {
                if (value != _ip)
                {
                    try
                    {
                        IPAddress.Parse(_ip);
                        _ip = value;
                    }
                    catch { }
                    OnPropertyChanged(nameof(Ip));
                }
            }
        }

        [UIProperty]
        public int Port { get; set; } = 5000;

        [UIProperty]
        public int MaxCountOfClients { get; set; } = 10;

        public TCPServerCreatorViewModel(Control model) : base(model)
        {
            Name = "tcp server";

            var viewModelProperties = GetType().GetProperties();

            foreach (var property in viewModelProperties)
            {
                if (property.GetCustomAttribute(typeof(UIPropertyAttribute)) != null)
                {
                    var row = new ListViewRow(property.Name);
                    row.AddElement(new TextBoxCellElement(this, property.Name, "Value"));

                    Properties.Add(row);
                }
            }
        }

        public Results AddSubport()
        {
            return Results.NotSupported;
        }

        public Results Create()
        {
            var port = new TCPServer()
            {
                Name = PortName,
                Ip = Ip,
                Port = Port,
                QueueSize = MaxCountOfClients
            };

            Model?.AddPort(port);

            return Results.Accept;
        }

        public void Remove()
        {

        }

        public void RemoveSubport()
        {

        }
    }

    public class TCPServerCreatorViewModel<TView> : TCPServerCreatorViewModel where TView : FrameworkElement, new()
    {
        public TCPServerCreatorViewModel(Control model) : base(model)
        {
            View = new TView();
            View.DataContext = this;

            var frameworkElement = new FrameworkElementFactory(typeof(TView));
            frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

            Template = new DataTemplate { VisualTree = frameworkElement };
        }
    }
}
