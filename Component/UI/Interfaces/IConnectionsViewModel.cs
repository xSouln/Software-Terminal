namespace Terminal.UI
{
    public interface IConnectionsViewModel
    {
        int Open();
        void Close();
        void Add();
        void Remove();
    }
}
