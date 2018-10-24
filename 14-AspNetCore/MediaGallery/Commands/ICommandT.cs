namespace MediaGallery.Commands
{
    public interface ICommand<T>
    {
        bool Execute(T parameter);
        bool Rollback();
    }
}
