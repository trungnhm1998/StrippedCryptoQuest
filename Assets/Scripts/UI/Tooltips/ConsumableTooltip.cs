using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Tooltips
{
    public class ConsumableTooltip : UITooltip
    {
        [SerializeField] private LocalizeStringEvent _displayNameString;
        public override UITooltip WithHeader(LocalizedString dataDisplayName)
        {
            _displayNameString.StringReference = dataDisplayName;
            return this;
        }
    }
}