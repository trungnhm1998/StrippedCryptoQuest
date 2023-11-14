using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Tooltips
{
    public class UIBeastToolTip : UITooltip
    {
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        public override ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            if (_localizedName == null) return this;
            _localizedName.StringReference = dataDisplayName;
            return this;
        }

        public override ITooltip WithLevel(int beastLevel)
        {
            _level.text = $"Lv. {beastLevel}";
            return this;
        }
    }
}