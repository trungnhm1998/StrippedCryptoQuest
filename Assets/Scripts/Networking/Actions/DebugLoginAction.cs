using CryptoQuest.Core;

namespace CryptoQuest.Networking.Actions
{
    public class DebugLoginAction : ActionBase { }

    public class AuthenticateSucceed : ActionBase { }

    public class LoginFinishedAction : ActionBase { }

    public class LoginFailedAction : ActionBase { }

    public class LoginUsingEmail : ActionBase { }

    public class AuthenticateUsingEmail : ActionBase
    {
        public string Email { get; }
        public string Password { get; }

        public AuthenticateUsingEmail(string email, string password)
        {
            Password = password;
            Email = email;
        }
    }

    public class LoginUsingFacebook : ActionBase { }

    public class LoginUsingGoogle : ActionBase { }

    public class LoginUsingTwitter : ActionBase { }

    public class LoginUsingWallet : ActionBase { }

    public class LoginUsingGmail : ActionBase { }
}