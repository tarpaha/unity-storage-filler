using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Platform;

namespace Model
{
    public class FileSystem : IFileSystem
    {
        private readonly IPlatform _platform;
        private readonly string _dir;
        private readonly byte[] _buffer;

        public FileSystem(IPlatform platform, string dir)
        {
            _platform = platform;
            _dir = dir;
            _buffer = new byte[1 << 20];
        }

        public IEnumerable<IFileRecord> Dir()
        {
            return new DirectoryInfo(_dir)
                .GetFiles()
                .Select(fileInfo => (IFileRecord) new FileRecord(fileInfo.Name, fileInfo.Length))
                .ToList();
        }

        public void Create(string name, long size)
        {
            using(var file = File.Create(_dir + name))
            {
                file.SetLength(size);
            }
            Changed();
        }

        public void Delete(string name)
        {
            File.Delete(_dir + name);
            Changed();
        }

        public event Action Changed = delegate { };

        public long FreeSpace { get { return _platform.FreeSpace; } }

        public long Total
        {
            get { return new DirectoryInfo(_dir).GetFiles().Sum(file => file.Length); }
        }
    }
}