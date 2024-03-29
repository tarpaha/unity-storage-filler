﻿using System;
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
            fileSystem.Progress += OnFileSystemOperationProgress;
            fileSystem.Changed += OnFileSystemChanged;
        }

        public void Process()
        {
            lock (_events)
            {
                while (_events.Count > 0)
                {
                    _events.Dequeue()();                    
                }
            }
        }

        private void OnFileSystemOperationProgress(float progress)
        {            
            lock (_events)
            {
                _events.Enqueue(() =>
                {
                    FileSystemOperationProgress(progress);                    
                });
            }
        }

        private void OnFileSystemChanged()
        {
            lock (_events)
            {
                _events.Enqueue(FilesChanged);
            }
        }

        public event Action<float> FileSystemOperationProgress = delegate { };
        public event Action FilesChanged = delegate { };
    }
}