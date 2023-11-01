using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Menus.Status.UI
{
    public class ConsumableTooltip : UITooltip
    {
        [SerializeField] private LocalizeStringEvent _displayNameString;
        public override ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            _displayNameString.StringReference = dataDisplayName;
            return this;
        }
    }
}
