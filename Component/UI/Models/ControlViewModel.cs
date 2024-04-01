using System.Collections.ObjectModel;
using xLibV100.Tracer.UI.View;
using xLibV100.UI;
using xLibV100.Windows;

namespace Terminal.UI
{
    public class ControlViewModel : ViewModelBase<Control, object>, IControlViewModel
    {
        public RelayCommand OpenBridgesViewCommand { get; private set; }

        protected WindowViewPresenter ConsoleViewPrecenter { get; set; }
        protected WindowViewPresenter BridgesViewPrecenter { get; set; }

        protected xTracerView ConsoleView = new xTracerView();
        protected ConnectionsViewModel ConnectionsViewModel { get; set; }
        protected BridgesOfPortsViewModel BridgesViewModel { get; set; }

        public ControlViewModel(Control control) : base(control)
        {
            Models = new ObservableCollection<object>();

            OpenBridgesViewCommand = new RelayCommand(OpenBridgesViewCommandHandler);

            /*foreach (TerminalObject device in control.Devices)
            {
                if (device.ViewModel != null)
                {
                    Models.Add(device.ViewModel);
                }
            }*/

            ConnectionsViewModel = new ConnectionsViewModel(control);
            BridgesViewModel = new BridgesOfPortsViewModel<BridgesOfPortsView>(control);

            View = new ControlView(this);
        }

        private void OpenBridgesViewCommandHandler(object obj)
        {
            OpenPortsBridges();
        }

        public override void Dispose()
        {
            base.Dispose();

            ConnectionsViewModel?.Dispose();

            ConsoleViewPrecenter?.Close();
            ConsoleViewPrecenter = null;

            BridgesViewPrecenter?.Close();
            BridgesViewPrecenter = null;

            BridgesViewModel?.Dispose();
        }

        public void OpenConsole()
        {
            if (ConsoleViewPrecenter == null)
            {
                ConsoleViewPrecenter = new WindowViewPresenter(ConsoleView);
                ConsoleViewPrecenter.Closed += (sender, e) =>
                {
                    ConsoleViewPrecenter = null;
                };

                ConsoleViewPrecenter.Show();
            }
            else
            {
                ConsoleViewPrecenter.Activate();
            }
        }

        public void OpenConnections()
        {
            ConnectionsViewModel?.Open();
        }

        public void OpenPortsBridges()
        {
            if (BridgesViewPrecenter == null)
            {
                BridgesViewPrecenter = new WindowViewPresenter(BridgesViewModel.UIModel);
                BridgesViewPrecenter.Closed += (sender, e) =>
                {
                    BridgesViewPrecenter = null;
                };

                BridgesViewPrecenter.Show();
            }
            else
            {
                BridgesViewPrecenter.Activate();
            }
        }
    }
}
