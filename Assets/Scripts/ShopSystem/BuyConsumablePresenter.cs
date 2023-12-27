using CryptoQuest.Item.Consumable;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class BuyConsumablePresenter : MonoBehaviour
    {
        [SerializeField] private ConsumableSO[] _sellingItems;
        [SerializeField] private UIEquipmentList _uiEquipmentList;
    }
}