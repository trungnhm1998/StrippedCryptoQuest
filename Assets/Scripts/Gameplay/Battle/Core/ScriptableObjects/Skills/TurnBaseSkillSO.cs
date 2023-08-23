using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "TurnBaseSkill", menuName = "Gameplay/Battle/Abilities/Turn Base Ability")]
    public class TurnBaseSkillSO : CQSkillSO
    {
        [SerializeField] private SkillParameters _parameters;

        public VoidEventChannelSO TurnEndEventChannel;
        protected override GameplayAbilitySpec CreateAbility()
        {
            var skill = new TurnBaseSkill();
            return skill;
        }
    }

    public class TurnBaseSkill : CQSkill
    {
        private int _turnLeft;
        protected new TurnBaseSkillSO AbilitySO => (TurnBaseSkillSO) _abilitySO;

        public override void OnAbilityGranted(Ability skill)
        {
            base.OnAbilityGranted(skill);
            // _turnLeft = Parameters.ContinuesTurn;
            AbilitySO.TurnEndEventChannel.EventRaised += OnTurnEndEvent;
        }

        public override void OnAbilityRemoved(GameplayAbilitySpec skill)
        {
            base.OnAbilityRemoved(skill);
            AbilitySO.TurnEndEventChannel.EventRaised -= OnTurnEndEvent;
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
            // _turnLeft = Parameters.ContinuesTurn;
        }
        
        private void OnTurnEndEvent()
        {
            if (!IsActive) return;
            // Compare the before value because caculation start at the next turn
            if (_turnLeft > 0)
            { 
                _turnLeft--;
                return; 
            }
            EndAbility();
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }

        protected override void SkillActivatePromt()
        {
            var actionData = AbilitySO.ActionDataSO;
            if (actionData == null) return; 

            actionData.Log.Clear();
            actionData.AddStringVar(UNIT_NAME_VARIABLE, _unit.UnitInfo.DisplayName);
            actionData.AddStringVar(SKILL_NAME_VARIABLE, AbilitySO.SkillName.GetLocalizedString());
            AbilitySO.ActionEventSO.RaiseEvent(actionData);
        }
    }
}