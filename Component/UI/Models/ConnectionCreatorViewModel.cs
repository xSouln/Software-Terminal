using xLibV100.UI;
using xLibV100.Controls;

namespace Terminal.UI
{
    public class ConnectionCreatorViewModel : ViewModelBase<Control, ConnectionCreatorView>, IConnectionCreatorViewModel
    {
        public delegate void NewCreationEventHandler(object arg, IConnectionCreatorViewModels viewModel);
        public event NewCreationEventHandler NewCreationEvent;

        public ConnectionCreatorViewModel(Control model) : base(model)
        {
            Name = "connection creator";

            Models.Add(new SerialPortCreatorViewModel(model));
            Models.Add(new TcpClientCreatorViewModel(model));
            Models.Add(new TCPServerCreatorViewModel<TcpServerCreatorView>(model));
            Models.Add(new MqttBrokerCreatorViewModel(model));
            Models.Add(new MqttClientCreatorViewModel(model));

            View = new ConnectionCreatorView(this);
            View.DataContext = this;

            UIModel = View;
        }

        public object SelectedValue { get; set; }

        public Results Apply()
        {
            IConnectionCreatorViewModels creator = SelectedValue as IConnectionCreatorViewModels;

            if (creator != null)
            {
                creator.Create();

                NewCreationEvent?.Invoke(this, creator);

                return Results.Accept;
            }

            return Results.Error;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Close()
        {
            foreach (var element in Models)
            {
                if (element is IConnectionCreatorViewModels viewModel)
                {
                    viewModel.Close();
                }
            }
        }
    }
}
