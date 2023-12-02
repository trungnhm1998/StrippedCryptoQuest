using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.MagicStone
{
    public class FetchMagicStones : ActionBase
    {
        public EMagicStoneStatus Status { get; set; } = EMagicStoneStatus.All;
    }

    public class FetchMagicStonesSucceeded : ActionBase
    {
        public List<Objects.MagicStone> InGameHeroes { get; }

        public FetchMagicStonesSucceeded(List<Objects.MagicStone> inGameHeroes)
        {
            InGameHeroes = inGameHeroes;
        }
    }

    public class FetchMagicStonesFailed : ActionBase { }
}