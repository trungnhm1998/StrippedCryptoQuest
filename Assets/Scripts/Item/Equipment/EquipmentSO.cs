using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Crypto Quest/Item/Equipment")]
    public class EquipmentSO : ScriptableObject
    {
        [field: SerializeField] public EquipmentData Data { get; private set; }
    }
}