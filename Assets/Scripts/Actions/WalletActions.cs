using CryptoQuest.Core;

namespace CryptoQuest.Actions
{
    public class ConnectWallet : ActionBase { }

    public class ConnectWalletCompleted : ActionBase
    {
        public bool IsSuccess { get; set; }

        public ConnectWalletCompleted(bool result) { IsSuccess = result; }
    }

    public class DisconnectWallet : ActionBase { }
    public class DisconnectWalletCompleted : ActionBase
    {
        public bool IsSuccess { get; set; }

        public DisconnectWalletCompleted(bool result) { IsSuccess = result; }
    }

}