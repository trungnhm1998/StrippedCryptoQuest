using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.MagicStone
{
    public class FetchProfileMagicStonesAction : ActionBase { }

    public class FetchMagicStonesSucceeded : ActionBase
    {
        public List<Objects.MagicStone> InGameStones { get; }

        public FetchMagicStonesSucceeded(List<Objects.MagicStone> inGameStones)
        {
            InGameStones = inGameStones;
        }
    }

    public class FetchMagicStonesFailed : ActionBase { }

    public class StoneInventoryFilled : ActionBase { }

    public class FetchDBoxMagicStonesSucceeded : ActionBase
    {
        public List<Objects.MagicStone> InGameStones { get; }

        public FetchDBoxMagicStonesSucceeded(List<Objects.MagicStone> inGameStones)
        {
            InGameStones = inGameStones;
        }
    }
}