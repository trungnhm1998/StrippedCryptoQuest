using System.Collections.Generic;
using IndiGames.Core.Events;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Sagas.Character
{
    public class FetchProfileCharactersAction : ActionBase { }

    public class HeroAction : ActionBase
    {
        public Obj.Character[] Heroes { get; }

        protected HeroAction(Obj.Character[] heroes)
        {
            Heroes = heroes;
        }
    }

    public class GetInGameHeroesSucceeded : HeroAction
    {
        public GetInGameHeroesSucceeded(Obj.Character[] heroes) : base(heroes) { }
    }

    public class GetInDboxHeroesSucceeded : HeroAction
    {
        public GetInDboxHeroesSucceeded(Obj.Character[] heroes) : base(heroes) { }
    }
    
    public class TransferCharactersAction : ActionBase
    {
        public int[] SelectedInDboxCharacters { get; }
        public int[] SelectedInGameCharacters { get; }

        public TransferCharactersAction(int[] selectedInGameCharacters, int[] selectedInDboxCharacters)
        {
            SelectedInGameCharacters = selectedInGameCharacters;
            SelectedInDboxCharacters = selectedInDboxCharacters;
        }
    }

    public class TransferSucceed : HeroAction
    {
        public TransferSucceed(Obj.Character[] heroes) : base(heroes) { }
    }

    public class TransferFailed : ActionBase { }
}