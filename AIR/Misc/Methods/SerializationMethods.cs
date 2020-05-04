using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Mvc;

namespace Misc.Methods
{
    public static class SerializationMethods
    {

        public static FileStreamResult SerializeToBytes(this Controller controller, object serializable)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, serializable);
            stream.Seek(0, SeekOrigin.Begin);
            return controller.File(stream,"multipart/form-data", "serialized.data");
        }

        
        
    }
}