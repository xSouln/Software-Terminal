using xLibV100.UI.CellElements;
using System.Reflection;
using System.Windows;
using xLibV100.Ports;
using xLibV100.UI;

namespace Terminal.UI
{
    public class TCPServerOptionsViewModel : ViewModelBase<PortBase, FrameworkElement>, ITCPServerOptionsViewModel
    {
        public RelayCommand ToggleVisibilityCommand { get; protected set; }

        public Visibility Visibility { get; set; } = Visibility.Collapsed;

        public bool IsAvailable
        {
            get => Model.State == States.Idle;
        }

        public TCPServerOptionsViewModel(PortBase model) : base(model)
        {
            Name = "options";

            ToggleVisibilityCommand = new RelayCommand(ToggleVisibilityCommandHandler);
            Model.ConnectionStateChanged += ConnectionStateChangedHandler;

            var optionsProperties = model.GetType().GetProperties();
            foreach (var property in optionsProperties)
            {
                PortPropertyAttribute portPropertyAttribute = property.GetCustomAttribute<PortPropertyAttribute>();
                if (portPropertyAttribute != null && portPropertyAttribute.Key == "Options")
                {
                    var row = new ListViewRow(property.Name);
                    row.AddElement(new TextBoxCellElement(model, property.Name, "Value") { Parent = this });
                    Properties.Add(row);
                }
            }
        }

        private void ToggleVisibilityCommandHandler(object obj)
        {
            Visibility = Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(Visibility));

            OnUpdateEvent();
        }

        public override void Dispose()
        {
            base.Dispose();

            Model.ConnectionStateChanged -= ConnectionStateChangedHandler;
        }

        private void ConnectionStateChangedHandler(PortBase port, ConnectionStateChangedEventHandlerArg arg)
        {
            OnPropertyChanged(nameof(IsAvailable));
        }
    }

    public class TCPServerOptionsViewModel<TView> : TCPServerOptionsViewModel where TView : FrameworkElement
    {
        public TCPServerOptionsViewModel(PortBase model) : base(model)
        {
            View = new FrameworkElement();
            View.DataContext = model;

            var frameworkElement = new FrameworkElementFactory(typeof(TView));
            frameworkElement.SetValue(FrameworkElement.DataContextProperty, this);

            Template = new DataTemplate { VisualTree = frameworkElement };
        }
    }
}
