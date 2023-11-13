using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Tooltips
{
    public class UICharacterTooltip : UITooltip
    {
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        public override UITooltip WithHeader(LocalizedString localizedName)
        {
            if (localizedName == null) return this;
            _localizedName.StringReference = localizedName;
            return this;
        }

        public override UITooltip WithLevel(int charLevel)
        {
            _level.text = $"Lv. {charLevel}";
            return this;
        }
    }
}