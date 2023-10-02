using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectSkill;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToCastSkill : StateBase
    {
        public SelectSingleHeroToCastSkill(UISkill skillUI, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm) { }

        public override void OnEnter() { }

        public override void OnExit() { }
    }
}