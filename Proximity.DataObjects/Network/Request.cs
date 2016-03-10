using System;
using ProtoBuf;

namespace Proximity.DataObjects.Network {
    [Serializable]
    [ProtoContract]
    public class Request {
        [ProtoMember(1, IsRequired = true)]
        public RequestType Type { get; set; }

        [ProtoMember(2, IsRequired = false)]
        public byte[] Payload { get; set; }
    }
}
