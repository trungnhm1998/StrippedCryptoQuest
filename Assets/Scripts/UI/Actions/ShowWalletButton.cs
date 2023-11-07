using CryptoQuest.Core;

namespace CryptoQuest.UI.Actions
{
    public class ShowWalletButton : ActionBase
    {
        public bool IsShown { get; set; } = true;

        public ShowWalletButton(bool isShown = true)
        {
            IsShown = isShown;
        }
    }
}