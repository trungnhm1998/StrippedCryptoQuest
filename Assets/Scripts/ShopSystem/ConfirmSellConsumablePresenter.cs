using CryptoQuest.UI.Dialogs.ChoiceDialog;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class ConfirmSellConsumablePresenter : MonoBehaviour
    {
        [SerializeField] private UIQuantityDialog _quantityConfigDialog;
        [SerializeField] private UIShopItemPool<UIConsumableShopItem> _pool;
        [SerializeField] private LocalizedString _confirmString = new("ShopUI", "DIALOG_SELL_CONFIRM");
        [SerializeField] private SellPanel _sellPanel;

        private UIChoiceDialog _confirmDialog;

        private void Awake()
        {
            ChoiceDialogController.Instance.InstantiateAsync(dialog => _confirmDialog = dialog);
        }

        private void OnEnable()
        {
            UIConsumableShopItem.Pressed += ConfigQuantity;
        }

        private void OnDisable()
        {
            UIConsumableShopItem.Pressed -= ConfigQuantity;
        }

        private void ConfigQuantity(UIConsumableShopItem item)
        {
            _sellPanel.DisableInput();
            var currentScrollRect = GetComponentInChildren<ScrollRect>();
            var selectables = currentScrollRect.GetComponentsInChildren<Selectable>();
            foreach (var selectable in selectables) selectable.interactable = false;

            _quantityConfigDialog
                .WithQuantityChangedCallback(quantity =>
                {
                    _confirmString["PRICE"] = new StringVariable { Value = $"{item.Price * quantity}G" };
                    _confirmDialog.SetMessage(_confirmString);
                })
                .Show(item.Info.Quantity);

            _confirmDialog
                .WithNoCallback(() => EventSystem.current.SetSelectedGameObject(item.gameObject))
                .WithYesCallback(() =>
                {
                    var transformParent = item.transform.parent;
                    var childCount = transformParent.childCount;
                    var itemIndex = item.transform.GetSiblingIndex();

                    var selectedQuantity = _quantityConfigDialog.CurrentQuantity;
                    item.Info.SetQuantity(item.Info.Quantity - selectedQuantity);
                    item.Render(item.Info);
                    if (item.Info.Quantity <= 0) _pool.Release(item);

                    if (childCount == 1) return;
                    var childToSelect = itemIndex == childCount - 1
                        ? transformParent.GetChild(itemIndex - 1).gameObject
                        : transformParent.GetChild(itemIndex).gameObject;

                    EventSystem.current.SetSelectedGameObject(childToSelect);
                })
                .WithHideCallback(() =>
                {
                    _quantityConfigDialog.Hide();
                    _sellPanel.EnableInput();
                    foreach (var selectable in selectables) selectable.interactable = true;
                })
                .SetMessage(_confirmString)
                .Show();

            var button = item.GetComponent<Button>();
            button.image.overrideSprite = button.spriteState.pressedSprite;
        }
    }
}