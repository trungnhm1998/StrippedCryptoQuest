using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Item;
using TinyMessenger;

namespace CryptoQuest.Battle.UI.SelectItem
{
    public class UIItemButton : UICommandDetailButton, IButtonUI
    {
        // I use this to save state when player select command
        private ConsumableInfo _originItem;
        private ConsumableInfo _cloneItem;

        private TinyMessageSubscriptionToken _roundEndedEvent;
        private TinyMessageSubscriptionToken _selectedItemEvent;
        private TinyMessageSubscriptionToken _cancelSelectedItemEvent;

        private void Awake()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        public override void Init(ButtonInfoBase info, int index)
        {
            base.Init(info, index);
            info.Accept(this);
        }

        public void Visit(ItemButtonInfo info)
        {
            _originItem = info.Item;
            _cloneItem = new ConsumableInfo(_originItem.Data, _originItem.Quantity);
        }

        private void SetQuantityText(ConsumableInfo item)
        {
            if (item == null || !item.IsValid()) return;
            _value.text = $"x{item.Quantity}";
        }

        private void SubscribeEvents()
        {
            _selectedItemEvent = BattleEventBus.SubscribeEvent<SelectedItemEvent>(SelectedItem);
            _cancelSelectedItemEvent = BattleEventBus.SubscribeEvent<CancelSelectedItemEvent>(CancelSelectedItem);
            _roundEndedEvent = BattleEventBus.SubscribeEvent<RoundEndedEvent>(ResetToOriginal);
        }

        private void UnsubscribeEvents()
        {
            BattleEventBus.UnsubscribeEvent(_selectedItemEvent);
            BattleEventBus.UnsubscribeEvent(_cancelSelectedItemEvent);
            BattleEventBus.UnsubscribeEvent(_roundEndedEvent);
        }

        private void SelectedItem(SelectedItemEvent eventObject)
        {
            UpdateCloneItem(eventObject.ItemInfo, -1);
        }

        private void CancelSelectedItem(CancelSelectedItemEvent eventObject)
        {
            UpdateCloneItem(eventObject.ItemInfo, 1);
        }

        private void ResetToOriginal(RoundEndedEvent eventObject)
        {
            _cloneItem = new ConsumableInfo(_originItem.Data, _originItem.Quantity);
            UpdateCloneItem(_cloneItem, 0);
        }

        /// <summary>
        /// Update clone item quantity and show in UI or disable if there's no item
        /// I don't release or destroy here because there's case when player cancel command 
        /// or use item fail, the item will show up again 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updateValue"></param>
        private void UpdateCloneItem(ConsumableInfo item, int updateValue)
        {
            if (_cloneItem.Data == null || _cloneItem.Data != item.Data) return;
            _cloneItem.SetQuantity(_cloneItem.Quantity + updateValue);
            if (_cloneItem.Quantity <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            SetQuantityText(_cloneItem);
        }
    }
}