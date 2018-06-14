using System;

namespace Model
{
    public class FileRecord : IFileRecord
    {
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
        public long Size { get; private set; }

        public FileRecord(string name, DateTime date, long size)
        {
            Name = name;
            Date = date;
            Size = size;
        }
    }
}