using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Variant
{
    public abstract class VariantBaseSO : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public LocalizedString Name { get; private set; }
    }
}