using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Character
{
    [Serializable]
    public struct HeroSkillsSet
    {
        [field: SerializeField, FormerlySerializedAs("SkillId")] public int Skill { get; set; }
        [field: SerializeField, FormerlySerializedAs("Level")] public int Level { get; set; }
        [field: SerializeField, FormerlySerializedAs("ClassId")] public int Class { get; set; }
        [field: SerializeField, FormerlySerializedAs("ElementId")] public int Element { get; set; }
    }
}