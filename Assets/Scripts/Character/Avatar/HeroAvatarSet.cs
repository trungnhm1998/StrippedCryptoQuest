using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Character.Avatar
{
    [Serializable]
    public struct HeroAvatarSet
    {
        [field: SerializeField, FormerlySerializedAs("CharacterId")] public int CharacterId { get; set; }
        [field: SerializeField, FormerlySerializedAs("ClassId")] public int ClassId { get; set; }
        [field: SerializeField, FormerlySerializedAs("ImageName")] public string ImageName { get; set; }
    }
}
