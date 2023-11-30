using System;

namespace CryptoQuest.Battle.ScriptableObjects
{
    [Flags]
    public enum EAbilityUsageScenario
    {
        Field = 1, // 000001
        Battle = 2, // 000010
    }
}