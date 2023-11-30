using System.Collections.Generic;
using IndiGames.Core.Events;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Character.Sagas
{
    public class GetInGameHeroes : ActionBase
    {
        public Obj.ECharacterStatus Status { get; set; } = Obj.ECharacterStatus.All;
    }

    public class FetchInGameHeroesSucceeded : ActionBase
    {
        public List<Obj.Character> InGameHeroes { get; }

        public FetchInGameHeroesSucceeded(List<Obj.Character> inGameHeroes)
        {
            InGameHeroes = inGameHeroes;
        }
    }

    public class FetchInGameHeroesFailed : ActionBase { }
}