using System;
using System.Collections.Generic;

namespace Model
{
    public interface IFileSystem
    {
        IEnumerable<IFileRecord> Dir();
        void Create(string name, long size);
        void Delete(string name);

        event Action Changed;
        
        long FreeSpace { get; } 
        long Total { get; }
    }
}