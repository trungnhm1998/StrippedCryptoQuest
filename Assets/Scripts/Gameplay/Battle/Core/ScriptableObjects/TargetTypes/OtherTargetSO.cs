using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.TargetTypes
{
    [CreateAssetMenu(fileName = "SelfTargetSO", menuName = "Gameplay/Battle/Abilities/Target Type/Other Target")]
    public class OtherTargetSO : TargetType
    {
        [SerializeField] private TargetContainterSO _targetContainer;
        public override void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets)
        {
            targets.AddRange(_targetContainer.Targets);
        }
    }

}