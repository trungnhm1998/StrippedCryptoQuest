using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;
using UnityEngine;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.TargetTypes
{
    [CreateAssetMenu(fileName = "BattleUnitTargetSO", menuName = "Gameplay/Battle/Abilities/Target Type/Battle Unit Target")]
    public class BattleUnitTargetSO : TargetType
    {
        public override void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets)
        {
            if (!owner.TryGetComponent<BattleUnitBase>(out var unit)) return;
            targets.AddRange(unit.TargetContainer);
        }
    }

}