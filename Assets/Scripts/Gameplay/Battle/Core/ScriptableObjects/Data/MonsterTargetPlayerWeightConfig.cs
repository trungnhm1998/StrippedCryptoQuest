using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "MonsterTargetPlayerWeightConfig",
        menuName = "Gameplay/Battle/Data/Monster Target Player Weight Config")]
    public class MonsterTargetPlayerWeightConfig : ScriptableObject 
    {
        [Tooltip("Weight to decide which hero will monster target. Lenght should be more than hero slot.")]
        [field: SerializeField]
        public int[] Weights { get; private set; }
    }
}