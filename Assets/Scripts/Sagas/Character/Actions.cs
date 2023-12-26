using System.Collections.Generic;
using IndiGames.Core.Events;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Sagas.Character
{
    public class FetchProfileCharactersAction : ActionBase { }

    public class HeroAction : ActionBase
    {
        public List<Obj.Character> Heroes { get; }

        public HeroAction(List<Obj.Character> heroes)
        {
            Heroes = heroes;
        }
    }

    public class GetInGameHeroesSucceeded : HeroAction
    {
        public GetInGameHeroesSucceeded(List<Obj.Character> heroes) : base(heroes) { }
    }

    public class GetInDboxHeroesSucceeded : HeroAction
    {
        public GetInDboxHeroesSucceeded(List<Obj.Character> heroes) : base(heroes) { }
    }

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
    
    public class TransferSucceed : ActionBase
    {
        public Obj.Character[] ResponseCharacters { get; }

        public TransferSucceed(Obj.Character[] responseCharacters)
        {
            ResponseCharacters = responseCharacters;
        }
    }
}