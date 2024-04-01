using System.Windows;

namespace Terminal.UI
{
    public interface IPortInfoViewModel
    {
        DataTemplate InfoTemplate { get; }
        Visibility InfoVisibility { get; }
        bool InfoIsAvailable { get; }
        void ToggleInfoVisibility();
    }
}
