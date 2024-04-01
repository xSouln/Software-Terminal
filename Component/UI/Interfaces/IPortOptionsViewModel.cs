using System.Windows;

namespace Terminal.UI
{
    public interface IPortOptionsViewModel
    {
        DataTemplate OptionsTemplate { get; }
        Visibility OptionsVisibility { get; }
        bool OptionsIsAvailable { get; }
        void ToggleOptionsVisibility();
    }
}
