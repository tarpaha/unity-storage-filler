using System;

namespace Model
{
    public interface IEvents
    {
        void Process();
        
        event Action FilesChanged;
    }
}