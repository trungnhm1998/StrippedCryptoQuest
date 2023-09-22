using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item
{
    public class RaritySO : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public LocalizedString DisplayName { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}