using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    public enum EEffectType
    {
        Damage = 1,
        Buff = 2,
        DeBuff = 3,
        AddStateChange = 4,
        RemoveAbnormalStatus = 5,
        Absorption = 6,
        Reflect = 7,
        Nullify = 8,
        Restore = 9,
        Special = 99,
    }

    [CreateAssetMenu(fileName = "SkillEffectType", menuName = "Gameplay/Battle/Data/SkillEffectType")]
    public class SkillEffectType : GenericData { }
}