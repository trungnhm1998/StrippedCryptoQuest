using CryptoQuest.Core;
using CryptoQuest.Sagas;

namespace CryptoQuest.Networking.Actions
{
    public class ConnectWallet : ActionBase { }

    public class ConnectWalletCompleted : ActionBase
    {
        public bool IsSuccess { get; set; }

        public ConnectWalletCompleted(bool result) { IsSuccess = result; }
    }

    public class DisconnectWallet : ActionBase { }
    public class DisconnectWalletWalletCompleted : ActionBase
    {
        public bool IsSuccess { get; set; }

        public DisconnectWalletWalletCompleted(bool result) { IsSuccess = result; }
    }

}