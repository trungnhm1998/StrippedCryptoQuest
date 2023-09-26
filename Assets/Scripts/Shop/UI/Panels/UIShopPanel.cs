using CryptoQuest.Shop.UI.ScriptableObjects;
using FSM;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Panels
{
    public abstract class UIShopPanel : MonoBehaviour
    {
        [field: SerializeField] public ShopStateSO State { get; private set; }
        [field: SerializeField] public LocalizedString DiaglogMessage { get; protected set; }
        [SerializeField] protected GameObject _content;

        public void Show()
        {
            _content.SetActive(true);
            OnShow();
        }

        protected virtual void OnShow() { }

        public void Hide()
        {
            _content.SetActive(false);
            OnHide();
        }

        protected virtual void OnHide() { }

        public abstract StateBase<string> GetPanelState(ShopManager shopManager);
    }
}
