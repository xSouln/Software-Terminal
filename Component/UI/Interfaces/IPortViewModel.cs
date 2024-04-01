using System.Windows;
using System.Windows.Media;

namespace Terminal.UI
{
    public interface IPortViewModel : ICloseableViewModel
    {
        DataTemplate ActionTemplate { get; }
        bool ConnectionIsAvailable { get; }
        string ConnectionStateName { get; }
        Brush ConnectionStateBackground { get; }
        void ConnectAction();
        void StartAction();
    }
}
