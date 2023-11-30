using IndiGames.Core.Events;

namespace CryptoQuest.UI.Actions
{
    public class ShowLoading : ActionBase
    {
        public bool IsShown { get; set; } = true;

        public ShowLoading(bool isShown = true)
        {
            IsShown = isShown;
        }
    }
}