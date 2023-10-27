using CryptoQuest.Core;

namespace CryptoQuest.Networking.Actions
{
    public class LoginAction : ActionBase
    {
        public string Email { get; }
        public string Pass { get; }

        public LoginAction() { }

        public LoginAction(string email, string pass)
        {
            Email = email;
            Pass = pass;
        }
    }

    public class LoginFinishedAction : ActionBase { }

    public class LoginFailedAction : ActionBase { }
}