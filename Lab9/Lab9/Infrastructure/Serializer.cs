using System.IO;

namespace Lab9.Infrastructure;

public static class Serializer
{
    public static void SaveToBinaryFile<T>(string path, T obj)
    {
        using (var fs = new FileStream(path, FileMode.Create))
        using (var bw = new BinaryWriter(fs))
        {
            // Примерный способ записи данных
        }
    }
}