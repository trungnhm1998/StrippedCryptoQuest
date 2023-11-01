using CryptoQuest.Gameplay.PlayerParty;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SavePartyAction : SaveActionBase<PartyManager>
    {
        public SavePartyAction(PartyManager obj) : base(obj)
        {
        }
    }

    public class LoadPartyAction : SaveActionBase<PartyManager>
    {
        public LoadPartyAction(PartyManager obj) : base(obj)
        {
        }
    }

    public class SavePartyCompletedAction : SaveCompletedActionBase
    {
        public SavePartyCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadPartyCompletedAction : SaveCompletedActionBase
    {
        public LoadPartyCompletedAction(bool result) : base(result)
        {
        }
    }
}