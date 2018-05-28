using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Class stores occured events and fires them in a call to Process()
    /// This allows to react to all events in one (main) thread
    /// </summary>
    public class MainThreadEvents : IEvents
    {
        private volatile Queue<Action> _events = new Queue<Action>();
        
        public MainThreadEvents(IFileSystem fileSystem)
        {
            fileSystem.Changed += OnFileSystemChanged;
        }

        public void Process()
        {
            lock (_events)
            {
                if (_events.Count > 0)
                {
                    _events.Dequeue()();                    
                }
            }
        }

        private void OnFileSystemChanged()
        {
            lock (_events)
            {
                _events.Enqueue(FilesChanged);
            }
        }

        public event Action FilesChanged = delegate { };
    }
}