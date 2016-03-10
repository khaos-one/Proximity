using System;
using ProtoBuf;

namespace Proximity.DataObjects.Network {
    [Serializable]
    [ProtoContract]
    public class Response {
        [ProtoMember(1, IsRequired = true)]
        public ResponseStatus Status { get; set; }

        [ProtoMember(2, IsRequired = false)]
        public byte[] Payload { get; set; }
    }
}
