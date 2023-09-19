using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EscapeBehaviour : CharacterComponentBase
    {
        [SerializeField] private EscapeAbility _escapeAbility;

        private EscapeAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<EscapeAbilitySpec>(_escapeAbility);
        }

        public void Escape(float highestEnemySpeed)
        {
            _spec.Execute(highestEnemySpeed);
        }
    }
}