using System.Collections.Generic;
using CryptoQuest.Core;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch
{
    public class GetBeasts : ActionBase
    {
        public Obj.EBeastStatus Status { get; set; } = Obj.EBeastStatus.All;
    }

    public class GetWalletNftBeastsSucceed : ActionBase
    {
        public List<Obj.Beast> WalletBeasts { get; }

        public GetWalletNftBeastsSucceed(List<Obj.Beast> walletBeasts)
        {
            WalletBeasts = walletBeasts;
        }
    }
    
    public class GetNftBeastsFailed : ActionBase { }
    
    public class SendBeastsToWallet : ActionBase
    {
        public int[] SelectedInGameBeasts { get; }

        public SendBeastsToWallet(int[] selectedInGameBeasts)
        {
            SelectedInGameBeasts = selectedInGameBeasts;
        }
    }
    
    public class SendBeastsToGame : ActionBase
    {
        public int[] SelectedInWalletBeasts { get; }

        public SendBeastsToGame(int[] selectedInWalletBeasts)
        {
            SelectedInWalletBeasts = selectedInWalletBeasts;
        }
    }
    
    public class SendBeastsToBothSide : ActionBase
    {
        public int[] SelectedInWalletBeasts { get; }
        public int[] SelectedInGameBeasts { get; }

        public SendBeastsToBothSide(int[] selectedInGameBeasts, int[] selectedInWalletBeasts)
        {
            SelectedInGameBeasts = selectedInGameBeasts;
            SelectedInWalletBeasts = selectedInWalletBeasts;
        }
    }
    
    public class TransferSucceed : ActionBase
    {
        public Obj.Beast[] ResponseBeasts { get; }

        public TransferSucceed(Obj.Beast[] responseBeasts)
        {
            ResponseBeasts = responseBeasts;
        }
    }
   
    public class TransferFailed : ActionBase { }
    
    public class GetGameNftBeastsSucceed : ActionBase
    {
        public List<Obj.Beast> InGameBeasts { get; }
        
        public GetGameNftBeastsSucceed(List<Obj.Beast> inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }
}