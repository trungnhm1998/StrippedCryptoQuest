using CryptoQuest.Actions;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.Skill.UI.TownTransfer
{
    public class TransferConsumableQuantity : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _showTownMenuEvent;
        [SerializeField] private VoidEventChannelSO _teleportSuccessEvent;
        [SerializeField] private ConsumableSO _townTransferItem;
        private TinyMessageSubscriptionToken _teleportCancelEventToken;

        private void OnEnable()
        {
            _showTownMenuEvent.EventRaised += Show;
            _teleportCancelEventToken = ActionDispatcher.Bind<TownTransferCancelEvent>(Hide);
        }

        private void OnDisable()
        {
            _showTownMenuEvent.EventRaised -= Show;
            _teleportSuccessEvent.EventRaised -= ConfirmUse;
            ActionDispatcher.Unbind(_teleportCancelEventToken);
        }

        private void Hide(TownTransferCancelEvent _)
        {
            _teleportSuccessEvent.EventRaised -= ConfirmUse;
        }

        private void Show()
        {
            _teleportSuccessEvent.EventRaised += ConfirmUse;
        }

        private void ConfirmUse()
        {
            ActionDispatcher.Dispatch(new ItemConsumed(_townTransferItem));
            _teleportSuccessEvent.EventRaised -= ConfirmUse;
        }
    }
}