using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    public class CharacterUnit : BattleUnitBase
    {
        public override void Init(BattleTeam team, AbilitySystemBehaviour owner)
        {
            base.Init(team, owner);
            UnitData.Owner = owner;
        }

        private void Start()
        {
            GrantDefaulSkills();
        }

        private void GrantDefaulSkills()
        {
            foreach (var skill in UnitData.GrantedSkills)
            {
                Owner.GiveAbility(skill);
            }
        }
    }
}