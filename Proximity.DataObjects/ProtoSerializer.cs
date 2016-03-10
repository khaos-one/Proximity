using System.IO;
using ProtoBuf;

namespace Proximity.DataObjects {
    public static class ProtoSerializer {
        public static T Deserialize<T>(Stream s) {
            return Serializer.Deserialize<T>(s);
        }

        public static T Deserialize<T>(byte[] data) {
            using (var ms = new MemoryStream(data)) {
                return Serializer.Deserialize<T>(ms);
            }
        }

        public static T DeserializeNetwork<T>(Stream s) {
            return Serializer.DeserializeWithLengthPrefix<T>(s, PrefixStyle.Base128);
        }

        public static void Serialize<T>(Stream s, T entity) {
            Serializer.Serialize(s, entity);
        }

        public static byte[] Serialize<T>(T entity) {
            using (var ms = new MemoryStream()) {
                Serializer.Serialize(ms, entity);
                return ms.ToArray();
            }
        }

        public static void SerializeNetwork<T>(Stream s, T entity) {
            Serializer.SerializeWithLengthPrefix(s, entity, PrefixStyle.Base128);
        }
    }
}
