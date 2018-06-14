using System;

namespace Model
{
    public interface IFileRecord
    {
        string Name { get; }
        DateTime Date { get; }
        long Size { get; }
    }
}