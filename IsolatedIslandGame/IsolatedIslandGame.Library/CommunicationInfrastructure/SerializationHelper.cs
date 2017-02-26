using IsolatedIslandGame.Library.Quests;
using MsgPack.Serialization;
using System.IO;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public static class SerializationHelper
    {
        public static T TypeDeserialize<T>(byte[] serializedData)
        {
            var serializer = MessagePackSerializer.Get<T>();
            using (MemoryStream ms = new MemoryStream(serializedData))
            {
                return serializer.Unpack(ms);
            }
        }
        public static byte[] TypeSerialize<T>(T data)
        {
            var serializer = MessagePackSerializer.Get<T>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Pack(memoryStream, data);
                return memoryStream.ToArray();
            }
        }

        public static QuestRecord QuestRecordDeserialize(byte[] serializedData)
        {
            var serializer = MessagePackSerializer.Get<QuestRecord>();
            using (MemoryStream ms = new MemoryStream(serializedData))
            {
                return serializer.Unpack(ms);
            }
        }
        public static byte[] QuestRecordSerialize(QuestRecord data)
        {
            var serializer = MessagePackSerializer.Get<QuestRecord>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Pack(memoryStream, data);
                return memoryStream.ToArray();
            }
        }

        public static Blueprint.ElementInfo[] BlueprintElementInfoArrayDeserialize(byte[] serializedData)
        {
            var serializer = MessagePackSerializer.Get<Blueprint.ElementInfo[]>();
            using (MemoryStream ms = new MemoryStream(serializedData))
            {
                return serializer.Unpack(ms);
            }
        }
        public static byte[] BlueprintElementInfoArraySerialize(Blueprint.ElementInfo[] data)
        {
            var serializer = MessagePackSerializer.Get<Blueprint.ElementInfo[]>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Pack(memoryStream, data);
                return memoryStream.ToArray();
            }
        }
    }
}
