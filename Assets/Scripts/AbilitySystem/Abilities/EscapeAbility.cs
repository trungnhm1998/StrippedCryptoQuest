using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Manager;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class EscapeAbility : CastSkillAbility
    {
        protected override GameplayAbilitySpec CreateAbility() =>
            new EscapeAbilitySpec(this);
    }

    public class EscapeAbilitySpec : CastSkillAbilitySpec
    {
        private EscapeAction _escapeAction;

        public EscapeAbilitySpec(EscapeAbility def) : base(def)
        {
            _escapeAction = new EscapeAction();
        }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            _escapeAction.OnEscapeSucceeded += EscapeSucceeded;
            _escapeAction.OnEscapeFailed += EscapeFailed;
            ActionDispatcher.Dispatch(_escapeAction);
        }

        protected override IEnumerator OnAbilityActive()
        {
            if (CheckCastSkillSuccess() == false) yield break;

            RegisterBattleEndedEvents();

            ExecuteAbility();

            EndAbility();
        }

        private void EscapeSucceeded()
        {
            ApplyCost();
            _escapeAction.OnEscapeSucceeded -= EscapeSucceeded;
            _escapeAction.OnEscapeFailed -= EscapeFailed;
        }

        private void EscapeFailed()
        {
            _escapeAction.OnEscapeSucceeded -= EscapeSucceeded;
            _escapeAction.OnEscapeFailed -= EscapeFailed;
        }
    }
}