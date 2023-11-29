using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    public class EquipmentDefSO : ScriptableObject
    {
        [field: SerializeField] public EquipmentData Data { get; private set; }
    }
}