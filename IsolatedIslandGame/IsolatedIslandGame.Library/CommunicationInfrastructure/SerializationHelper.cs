using MsgPack.Serialization;
using System.IO;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public static class SerializationHelper
    {
        public static object Deserialize<T>(byte[] serializedData)
        {
            var serializer = MessagePackSerializer.Get<T>();
            using (MemoryStream ms = new MemoryStream(serializedData))
            {
                return serializer.Unpack(ms);
            }
        }

        public static byte[] Serialize<T>(object data)
        {
            T serializationTarget = (T)data;
            var serializer = MessagePackSerializer.Get<T>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Pack(memoryStream, serializationTarget);
                return memoryStream.ToArray();
            }
        }
    }
}
