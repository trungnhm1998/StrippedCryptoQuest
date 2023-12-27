using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Party
{
    public class SavePartyAction : ActionBase
    {
        public int[] Party { get; }

        public SavePartyAction(int[] party)
        {
            Party = party;
        }
    }
}