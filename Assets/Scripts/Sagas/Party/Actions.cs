using CryptoQuest.Sagas.Character;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Party
{
    public class SyncPartyAction : ActionBase
    {
        public int[] Party { get; }
        public SyncPartyAction(int[] party) { Party = party; }
    }

    public class SavePartyAction : HeroAction
    {
        public SavePartyAction(Objects.Character[] heroes) : base(heroes) { }
    }
}