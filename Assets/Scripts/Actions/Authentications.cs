using CryptoQuest.Core;
using CryptoQuest.Sagas;

namespace CryptoQuest.Actions
{
    public class DebugLoginAction : ActionBase { }

    public class AuthenticateSucceed : ActionBase { }

    public class AuthenticateFailed : ActionBase { }

    public class LoginFinishedAction : ActionBase { }

    public class LoginFailedAction : ActionBase { }

    public class LoginUsingEmail : ActionBase { }

    public class AuthenticateWithBackendAction : ActionBase
    {
        public string Token { get; set; }
    }

    public class InternalAuthenticateAction : ActionBase
    {
        public CredentialResponse ResponseCredentialResponse { get; }

        public InternalAuthenticateAction(CredentialResponse responseCredentialResponse)
        {
            ResponseCredentialResponse = responseCredentialResponse;
        }
    }

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

    public class RegisterEmailAction : ActionBase
    {
        public string Email { get; }
        public string Password { get; }

        public RegisterEmailAction(string email, string password)
        {
            Password = password;
            Email = email;
        }
    }


    public class LoginUsingFacebook : ActionBase { }

    public class LoginUsingGoogle : ActionBase { }

    public class LoginUsingTwitter : ActionBase { }

    public class LoginUsingGmail : ActionBase { }
}