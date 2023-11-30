using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Beast.Avatar
{
    [Serializable]
    public class BeastAvatarData
    {
        [field: SerializeField] public int BeastId { get; set; }
        [field: SerializeField] public int ClassId { get; set; }
        [field: SerializeField] public int ElementId { get; set; }
        [field: SerializeField] public AssetReferenceT<Sprite> Image { get; set; }
    }
}