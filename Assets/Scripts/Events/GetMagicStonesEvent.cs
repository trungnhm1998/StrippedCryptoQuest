using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    public class GetMagicStonesEvent : GenericEventChannelSO<List<IMagicStone>> { }
}