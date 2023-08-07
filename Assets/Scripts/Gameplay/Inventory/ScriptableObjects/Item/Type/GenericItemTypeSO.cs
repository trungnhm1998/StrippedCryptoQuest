using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    public class GenericItemTypeSO : ScriptableObject
    {
        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}