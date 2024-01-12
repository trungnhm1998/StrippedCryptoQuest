using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using CryptoQuest.UI.Tooltips.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class UIEquipmentDetails : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIEquipmentShopItem _uiEquipmentShopItem;
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;

        public IEquipment Equipment => _uiEquipmentShopItem.Info;

        private void OnValidate()
        {
            if (_uiEquipmentShopItem == null)
                _uiEquipmentShopItem = GetComponent<UIEquipmentShopItem>();
        }
        
        public void OnSelected()
        {
            _showTooltipEvent.RaiseEvent(true);
        }
        
        private void OnDisable()
        {
            _showTooltipEvent.RaiseEvent(false);
        }
    }
}