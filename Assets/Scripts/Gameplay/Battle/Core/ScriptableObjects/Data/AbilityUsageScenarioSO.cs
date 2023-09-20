using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "AbilityUsageScenario", menuName = "Gameplay/Battle/Data/Ability Usage Scenario")]
    public class AbilityUsageScenarioSO : GenericData
    {
        /// <summary>
        /// Use SO instead of directly use enum so the refactor will be easier if needed
        /// </summary>
        [field: SerializeField] public EAbilityUsageScenario UsageScenario { get; private set; }
    }

    [Flags]
    public enum EAbilityUsageScenario
    {
        Field = 1,      // 000001
        Battle = 2,     // 000010
    }
}