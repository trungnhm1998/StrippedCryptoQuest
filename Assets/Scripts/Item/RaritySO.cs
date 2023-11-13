using UnityEngine;

namespace CryptoQuest.Item
{
    public class RaritySO : ScriptableObject
    {
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Color Color { get; private set; } = Color.grey;
    }
}