using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "TurnBaseSkill", menuName = "Indigames Ability System/Skills/Turn Base Skill")]
    public class TurnBaseSkillSO : EffectSkillSO
    {
        [SerializeField] private SkillParameters _parameters;
        public override AbilityParameters Parameters => _parameters;

        public VoidEventChannelSO TurnEndEventChannel;
        protected override AbstractSkill CreateSkill()
        {
            var skill = new TurnBaseSkill();
            return skill;
        }
    }

    public class TurnBaseSkill : EffectSkill
    {
        private int _turnLeft;
        protected new TurnBaseSkillSO SkillSO => (TurnBaseSkillSO) _skillSO;
        protected SkillParameters Parameters => (SkillParameters) _parameters; 

        public override void OnSkillGranted(AbstractSkill skillSpec)
        {
            SkillSO.TurnEndEventChannel.EventRaised += OnTurnEndEvent;
        }

        public override void OnSkillRemoved(AbstractSkill skillSpec)
        {
            SkillSO.TurnEndEventChannel.EventRaised -= OnTurnEndEvent;
        }

        protected override IEnumerator InternalActiveSkill()
        {
            yield return base.InternalActiveSkill();
            _turnLeft = Parameters.continuesTurn;
        }
        private void OnTurnEndEvent()
        {
            Debug.Log($"TurnBaseSkill::OnTurnEndEvent: {SkillSO.name} Turn Left: {_turnLeft}");
            if (--_turnLeft > 0) return;
            EndSkill();
        }

        public override bool CanActiveSkill()
        {
            return !IsActive && base.CanActiveSkill();
        }
    }
}