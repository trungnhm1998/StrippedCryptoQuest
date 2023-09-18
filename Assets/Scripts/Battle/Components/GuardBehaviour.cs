using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class GuardBehaviour : CharacterComponentBase
    {
        [SerializeField] private GuardAbility _guardAbility;

        private GuardAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<GuardAbilitySpec>(_guardAbility);
        }
        
        public void GuardUntilEndOfTurn()
        {
            _spec.TryActiveAbility();
        }
    }
}