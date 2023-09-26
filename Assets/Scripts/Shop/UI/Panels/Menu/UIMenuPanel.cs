using FSM;
using UnityEngine;
using CryptoQuest.Shop.UI.ShopStates.Menu;
using UnityEngine.Localization;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.Menu;
using System.Collections;

namespace CryptoQuest.Shop.UI.Panels.Menu
{
    public class UIMenuPanel : UIShopPanel
    {
        [SerializeField] private MultiInputButton _defaultButton;
        private ShopManager _shopManager;

        public override StateBase<string> GetPanelState(ShopManager shopManager)
        {
            _shopManager = shopManager;
            return new ShopMenuState(this);
        }

        protected override void OnShow()
        {
            base.OnShow();
            StartCoroutine(SelectButton());
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        IEnumerator SelectButton()
        {
            yield return null;
            _defaultButton.Select();
        }    

        public void RequestChangeState(ShopStateSO stage)
        {
            _shopManager.RequestChangeState(stage.name);
        }     

        public void ExitShop()
        {
            _shopManager.HideShop();
        }    
    }
}
