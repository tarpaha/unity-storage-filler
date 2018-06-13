using System;

namespace Model
{
    public interface IEvents
    {
        void Process();

        event Action<float> FileSystemOperationProgress;
        event Action FilesChanged;
    }
}