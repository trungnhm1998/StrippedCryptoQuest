using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class BuyEquipmentModel : MonoBehaviour
    {
        [SerializeField] private EquipmentSO[] _sellingItems;
        public IEquipment[] SellingItems => _sellingItems;
    }
}