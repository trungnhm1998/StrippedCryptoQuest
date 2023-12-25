using CryptoQuest.Character.Hero;
using CryptoQuest.Character.LevelSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class UIListItem : MonoBehaviour, ISelectHandler
    {
        public event UnityAction<HeroSpec> InspectCharacterEvent;

        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        private HeroSpec _cachedInfo;

        public void SetInfo(HeroSpec info)
        {
            _cachedInfo = info;
            _localizedName.StringReference = info.Origin.DetailInformation.LocalizedName;
            var levelCalculator = new LevelCalculator(info.Stats.MaxLevel);
            _level.text = $"Lv {levelCalculator.CalculateCurrentLevel(info.Experience)}";
            _localizedName.RefreshString();
        }

        public void OnSelect(BaseEventData _)
        {
            InspectCharacterEvent?.Invoke(_cachedInfo);
        }
    }
}