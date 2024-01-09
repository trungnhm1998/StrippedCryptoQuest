using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public interface IConsumableInfoProvider
    {
        ConsumableInfo ConsumableInfo { get; }
    }

    public class ConsumableItemProvider : MonoBehaviour, IConsumableInfoProvider
    {
        [SerializeField] private UIConsumableShopItem _uiConsumableShopItem;
        [SerializeField] private BoolEventChannelSO _showConsumableDetailsEvent;

        public ConsumableInfo ConsumableInfo => _uiConsumableShopItem.Info;

        private void OnValidate()
        {
            if (_uiConsumableShopItem == null)
                _uiConsumableShopItem = GetComponent<UIConsumableShopItem>();
        }
        
        public void OnSelected()
        {
            _showConsumableDetailsEvent.RaiseEvent(true);
        }
        
        private void OnDisable()
        {
            _showConsumableDetailsEvent.RaiseEvent(false);
        }
    }
}