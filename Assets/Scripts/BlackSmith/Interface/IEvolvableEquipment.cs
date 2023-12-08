using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IEvolvableEquipment
    {
        IEquipment Equipment { get; }
        Sprite Icon { get; }
        LocalizedString LocalizedName { get; }
        int Level { get; }
        int Stars { get; }
        int Gold { get; }
        float Metad { get; }
        Sprite Rarity { get; }
        int Rate { get; }
    }
}