using CryptoQuest.ShopSystem.Sagas;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class ConfirmSellEquipmentPresenter : MonoBehaviour
    {
        [SerializeField] private UIShopItemPool<UIEquipmentShopItem> _equipmentPool;
        [SerializeField] private LocalizedString _confirmString = new("ShopUI", "DIALOG_SELL_CONFIRM");
        [SerializeField] private SellPanel _sellPanel;
        [SerializeField] private UIEquipmentList _uiWeaponList;
        [SerializeField] private UIEquipmentList _uiEquipmentList;
        [SerializeField] private TransactionResultPanel _resultPanel;

        private UIChoiceDialog _confirmDialog;

        private void Awake()
        {
            ChoiceDialogController.Instance.InstantiateAsync(dialog => _confirmDialog = dialog);
        }

        private void OnEnable()
        {
            _uiEquipmentList.ItemSelected += ConfirmSellEquipment;
            _uiWeaponList.ItemSelected += ConfirmSellEquipment;
        }

        private void OnDisable()
        {
            _uiEquipmentList.ItemSelected -= ConfirmSellEquipment;
            _uiWeaponList.ItemSelected -= ConfirmSellEquipment;
        }

        private void ConfirmSellEquipment(UIEquipmentShopItem item)
        {
            _sellPanel.DisableInput();
            var currentScrollRect = GetComponentInChildren<ScrollRect>();
            var selectables = currentScrollRect.GetComponentsInChildren<Selectable>();
            foreach (var selectable in selectables) selectable.interactable = false;
            _confirmString["PRICE"] = new StringVariable { Value = item.PriceText };
            _confirmDialog
                .WithNoCallback(() => { EventSystem.current.SetSelectedGameObject(item.gameObject); })
                .WithYesCallback(() => { OnConfirmSell(item); })
                .WithHideCallback(() =>
                {
                    _sellPanel.EnableInput();
                    foreach (var selectable in selectables) selectable.interactable = true;
                })
                .SetMessage(_confirmString)
                .Show();
            var button = item.GetComponent<Button>();
            button.image.overrideSprite = button.spriteState.pressedSprite;
        }

        private void OnConfirmSell(UIEquipmentShopItem item)
        {
            ActionDispatcher.Dispatch(new SellEquipmentAction(item.Info, item.Price));
            ShowSellSuccess(item);
        }

        private void ShowSellSuccess(UIEquipmentShopItem item)
        {
            _resultPanel.AddHideCallback(() =>
                {
                    var transformParent = item.transform.parent;
                    var childCount = transformParent.childCount;
                    var itemIndex = item.transform.GetSiblingIndex();
                    EventSystem.current.SetSelectedGameObject(null);
                    _equipmentPool.Release(item);
                    if (childCount == 1) return;
                    var childToSelect = itemIndex == childCount - 1
                        ? transformParent.GetChild(itemIndex - 1).gameObject
                        : transformParent.GetChild(itemIndex).gameObject;

                    EventSystem.current.SetSelectedGameObject(childToSelect);
                })
                .ShowSuccess();
        }
    }
}