using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;
using Tesla.Logging;

namespace Proximity.Configuration {
    public sealed class NetUserValidator : UserNamePasswordValidator {
        public override void Validate(string userName, string password) {
            var user = Program.Service.Config.NetComm.Users.FirstOrDefault(
                x => x.UserName == userName && x.PasswordSha1 == password);

            Log.Entry(Priority.Info, $"Login attempt with username `{userName}`.");

            if (user == null) {
                Log.Entry(Priority.Warning, $"Login failed with username `{userName}`.");
                throw new FaultException("Specified user/password pair does not exist.");
            }

            Log.Entry(Priority.Info, $"Login for user `{userName}` successful.");
        }
    }
}
