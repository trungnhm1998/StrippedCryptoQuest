using System.Collections;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "TurnBaseSkill", menuName = "Indigames Ability System/Abilities/Turn Base Ability")]
    public class TurnBaseSkillSO : EffectAbilitySO
    {
        [SerializeField] private SkillParameters _parameters;
        public override AbilityParameters Parameters => _parameters;

        public VoidEventChannelSO TurnEndEventChannel;
        protected override AbstractAbility CreateAbility()
        {
            var skill = new TurnBaseSkill();
            return skill;
        }
    }

    public class TurnBaseSkill : EffectAbility
    {
        private int _turnLeft;
        protected new TurnBaseSkillSO AbilitySO => (TurnBaseSkillSO) _abilitySO;
        protected SkillParameters Parameters => (SkillParameters) _parameters; 

        public override void OnAbilityGranted(AbstractAbility skillSpec)
        {
            _turnLeft = Parameters.continuesTurn;
            AbilitySO.TurnEndEventChannel.EventRaised += OnTurnEndEvent;
        }

        public override void OnAbilityRemoved(AbstractAbility skillSpec)
        {
            AbilitySO.TurnEndEventChannel.EventRaised -= OnTurnEndEvent;
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
            _turnLeft = Parameters.continuesTurn;
        }
        
        private void OnTurnEndEvent()
        {
            if (!IsActive) return;
            Debug.Log($"TurnBaseSkill::OnTurnEndEvent: {AbilitySO.name} Turn Left: {_turnLeft}");
            // Compare the before value because caculation start at the next turn
            if (_turnLeft-- > 0) return;
            EndAbility();
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}