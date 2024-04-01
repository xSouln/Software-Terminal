using xLibV100.Controls;

namespace Terminal.UI
{
    public interface IConnectionCreatorViewModels
    {
        Results Create();
        Results AddSubport();
        void RemoveSubport();
        void Remove();
        void Close();
    }
}
