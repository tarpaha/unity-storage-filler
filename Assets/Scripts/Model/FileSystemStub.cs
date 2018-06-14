using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class FileSystemStub : IFileSystem
    {
        private readonly List<IFileRecord> _files = new List<IFileRecord>();
    
        public IEnumerable<IFileRecord> Dir()
        {
            return _files;
        }

        public void Create(string name, long size)
        {
            Progress(0);
            _files.Add(new FileRecord(name, DateTime.Now, size));
            Progress(1);
            Changed();
        }

        public void Delete(string name)
        {
            _files.RemoveAll(file => file.Name == name);
            Changed();
        }

        public event Action<float> Progress = delegate { };
        public event Action Changed = delegate { };

        public long FreeSpace
        {
            get { return 0; }
        }

        public long Total
        {
            get { return _files.Sum(file => file.Size); }
        }
    }
}