using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class RetreatBehaviour : CharacterComponentBase
    {
        [SerializeField] private RetreatAbility _retreatAbility;

        private RetreatAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<RetreatAbilitySpec>(_retreatAbility);
        }

        public void Retreat(float highestEnemySpeed)
        {
            _spec.Execute(highestEnemySpeed);
        }

#if UNITY_EDITOR
        public void Editor_SetAbility(RetreatAbility retreatAbility)
        {
            _retreatAbility = retreatAbility;
        }
#endif
    }
}