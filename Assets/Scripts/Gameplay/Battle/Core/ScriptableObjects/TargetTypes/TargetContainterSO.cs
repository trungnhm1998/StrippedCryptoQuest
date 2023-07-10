using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "TargetContainter", menuName = "Gameplay/Battle/Abilities/Target Containter")]
    public class TargetContainterSO : ScriptableObject
    {
        public List<AbilitySystemBehaviour> Targets {get; private set;} = new();

        public void SetSingleTarget(AbilitySystemBehaviour target)
        {
            ResetTargets();
            Targets.Add(target);
        }
        
        public void SetMultipleTargets(List<AbilitySystemBehaviour> targets)
        {
            ResetTargets();
            Targets.AddRange(targets);
        }

        public void ResetTargets()
        {
            Targets.Clear();
        } 
    }
}