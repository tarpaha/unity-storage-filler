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

        private const int BufferSize = 100 * (1 << 20);

        public FileSystem(IPlatform platform, string dir)
        {
            _platform = platform;
            _dir = dir;
            _buffer = new byte[BufferSize];
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
            var file = File.Create(_dir + name);
            while (size > _buffer.Length)
            {
                file.Write(_buffer, 0, _buffer.Length);
                size -= _buffer.Length;
            }
            file.Write(_buffer, 0, (int)size);
            file.Close();
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