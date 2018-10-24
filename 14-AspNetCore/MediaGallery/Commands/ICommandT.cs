using System.Collections.Generic;

namespace MediaGallery.Commands
{
    public interface ICommand<T>
    {
        List<string> Validate(T parameter);
        bool Execute(T parameter);
        bool Rollback();
    }
}
