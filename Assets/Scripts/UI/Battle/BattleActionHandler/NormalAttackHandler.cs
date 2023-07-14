using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.GameHandler;

namespace CryptoQuest.UI.Battle
{
    public class NormalAttackHandler : BattleActionHandler
    {
        public override object Handle(IBattleUnit currentUnit)
        {
            if (currentUnit == null) return currentUnit;
            AbilitySystemBehaviour currentUnitOwner = currentUnit.Owner;
            if (currentUnit.NormalAttack == null)
            {
                Debug.LogWarning($"This character dont have normal attack skill");
                return currentUnit;
            }
            currentUnit.SelectSkill(currentUnit.NormalAttack);
            return base.Handle(currentUnit);
        }
    }
}