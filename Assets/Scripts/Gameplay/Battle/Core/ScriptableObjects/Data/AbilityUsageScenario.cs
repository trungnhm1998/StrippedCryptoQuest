using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [Flags]
    public enum EAbilityUsageScenario
    {
        Field = 1, // 000001
        Battle = 2, // 000010
    }
}