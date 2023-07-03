using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;

namespace IndiGames.GameplayAbilitySystem
{
    [CreateAssetMenu(fileName = "SelfTargetSO", menuName = "Indigames Ability System/Abilities/Target Type/Other Target")]
    public class OtherTargetSO : TargetType
    {
        [SerializeField] private TargetContainterSO _targetContainer;
        public override void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets)
        {
            targets.AddRange(_targetContainer.Targets);
        }
    }

}