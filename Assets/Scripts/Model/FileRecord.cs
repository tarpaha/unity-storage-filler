namespace Model
{
    public class FileRecord : IFileRecord
    {
        public string Name { get; private set; }
        public long Size { get; private set; }

        public FileRecord(string name, long size)
        {
            Name = name;
            Size = size;
        }
    }
}