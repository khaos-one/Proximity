using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;

namespace Proximity.Configuration {
    public sealed class NetUserValidator : UserNamePasswordValidator {
        public override void Validate(string userName, string password) {
            var user = Program.Service.Config.NetComm.Users.FirstOrDefault(
                x => x.UserName == userName && x.PasswordSha1 == password);

            if (user == null) {
                throw new FaultException("Specified user/password pair does not exist.");
            }
        }
    }
}
