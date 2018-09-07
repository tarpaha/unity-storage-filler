using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Platform;

namespace Model
{
    public class FileSystem : IFileSystem
    {
        private readonly IPlatform _platform;
        private readonly string _dir;
        private readonly byte[] _buffer;

        private const int BufferSize = 10 * (1 << 20);

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
                .Select(fileInfo => (IFileRecord) new FileRecord(fileInfo.Name, fileInfo.CreationTime, fileInfo.Length))
                .ToList();
        }

        public void Create(string name, long size)
        {
            var k = 1.0f / size;
            Progress(0);
            try
            {
                using (var file = File.Create(_dir + name))
                {
                    while (size > _buffer.Length)
                    {
                        file.Write(_buffer, 0, _buffer.Length);
                        Thread.Sleep(1);
                        size -= _buffer.Length;
                        Progress(1.0f - k * size);
                    }

                    file.Write(_buffer, 0, (int) size);
                }
            }
            catch
            {
                // ignored
            }
            Progress(1);
            Changed();
        }

        public void Delete(string name)
        {
            File.Delete(_dir + name);
            Changed();
        }

        public event Action<float> Progress = delegate { };
        public event Action Changed = delegate { };

        public long FreeSpace { get { return _platform.FreeSpace; } }

        public long Total
        {
            get { return new DirectoryInfo(_dir).GetFiles().Sum(file => file.Length); }
        }
    }
}