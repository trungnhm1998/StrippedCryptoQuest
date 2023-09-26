using CryptoQuest.Shop.UI.ShopStates.Result;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Panels.Result
{
    public class UIResultPanel : UIShopPanel
    {
        [SerializeField] private LocalizedString _successMessage;
        [SerializeField] private LocalizedString _failMessage;

        public string PreState { get; private set; }

        public override StateBase<string> GetPanelState(ShopManager shopManager)
        {
            return new ShopResultState(this);
        }

        public void InitResult(bool hasSuccess, string lastState)
        {
            DiaglogMessage = hasSuccess ? _successMessage : _failMessage;
            PreState = lastState;
        }
    }
}
