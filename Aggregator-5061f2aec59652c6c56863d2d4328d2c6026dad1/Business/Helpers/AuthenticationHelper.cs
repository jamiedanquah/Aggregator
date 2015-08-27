using Aggregator.Objects;
using Aggregator.Objects.Configuration;

namespace Aggregator.Business.Helpers
{
    public class AuthenticationHelper
    {
        private const string LogonFailMessage = "User name or password invalid";
        private const string LogonDisabledMessage = "Your account has been disabled please contact support";

        public static AggregatorSetting CheckAuth(Settings settings, Authentication authentication)
        {
            // see if they are in the dictionary as if not they are not going to be a valid user
            if (!settings.AggregatorSettings.ContainsKey(authentication.UserName))
                return new AggregatorSetting
                {
                    Enabled = false,
                    LoginSuccess = false,
                    Error = LogonFailMessage
                };

            // get the settings from the dictionary
            var aggregatorSettings = settings.AggregatorSettings[authentication.UserName];

            // now we have there details check if there password is correct
            if (aggregatorSettings.Password != authentication.Password)
                return new AggregatorSetting
                {
                    Enabled = false,
                    LoginSuccess = false,
                    Error = LogonFailMessage
                };

            // now see if there account is disabled
            if (!aggregatorSettings.Enabled)
                return new AggregatorSetting
                {
                    Enabled = false,
                    LoginSuccess = false,
                    Warning = LogonDisabledMessage
                };

            // everything is fine
            aggregatorSettings.LoginSuccess = true;
            return aggregatorSettings;
        }
    }
}