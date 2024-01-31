using CryptoQuest.Character.Hero;
using CryptoQuest.Character.LevelSystem;
using CryptoQuest.Tavern.UI.Tooltip;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterListItem : MonoBehaviour, ITooltipHeroProvider
    {
        [SerializeField] private Image _imgClass;
        [SerializeField] private LocalizeStringEvent _strName;
        [SerializeField] private TMP_Text _txtLevel;
        [SerializeField] private string _lvlFormat;
        
        protected LocalizeStringEvent StrName => _strName;
        protected TMP_Text TxtLevel => _txtLevel;

        public HeroSpec Spec { get; private set; } = new();
        public virtual void Init(HeroSpec heroSpec)
        {
            Reset();
            Spec = heroSpec;
            if (heroSpec.IsValid() == false) return;
            var lvlCalculator = new LevelCalculator(heroSpec.Stats.MaxLevel);
            _imgClass.LoadSpriteAndSet(heroSpec.Class.Icon);
            _strName.StringReference = heroSpec.Origin.DetailInformation.LocalizedName;
            _strName.RefreshString();
            _txtLevel.text = string.Format(_lvlFormat, lvlCalculator.CalculateCurrentLevel(heroSpec.Experience));
        }

        protected virtual void Reset()
        {
            _imgClass.sprite = null;
            _txtLevel.text = string.Empty;
        }

        public HeroSpec Hero => Spec;
    }
}