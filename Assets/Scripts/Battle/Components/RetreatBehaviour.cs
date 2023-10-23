using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class RetreatBehaviour : CharacterComponentBase
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private RetreatAbility _retreatAbility;

        private RetreatAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<RetreatAbilitySpec>(_retreatAbility);
        }

        public void Retreat(float highestEnemySpeed)
        {
            _spec.CanRetreatBattle = _battleBus.CurrentBattlefield.CanRetreat;
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