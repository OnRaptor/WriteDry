using System.IO;
using System.Runtime.Serialization;

namespace WriteDry.Utils
{
    public static class DeepCopyWriter
    {
        public static T DeepCopy<T>(T obj)
        {
            using var ms = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(ms, obj);
            ms.Position = 0;
            return (T)serializer.ReadObject(ms);
        }
    }

}
