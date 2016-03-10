using ProtoBuf;

namespace Proximity.DataObjects {
    [ProtoContract]
    public sealed class UserLoginCredentials {
        [ProtoMember(1, IsRequired = true)]
        public string UserName { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string PasswordSha1 { get; set; }
    }
}
