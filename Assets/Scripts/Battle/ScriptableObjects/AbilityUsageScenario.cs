using System;

namespace CryptoQuest.Battle.ScriptableObjects
{
    [Flags]
    public enum EAbilityUsageScenario
    {
        Field = 1, // 000001
        Battle = 2, // 000010
        World = 4, // 000100 
    }
}