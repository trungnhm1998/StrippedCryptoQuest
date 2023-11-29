using CryptoQuest.Character.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIListItem : MonoBehaviour, ISelectHandler
    {
        public static event UnityAction<HeroSpec> InspectCharacterEvent;

        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        private HeroSpec _cachedInfo;

        public void SetInfo(HeroSpec info)
        {
            _cachedInfo = info;
            _localizedName.StringReference = info.Origin.DetailInformation.LocalizedName;
            _level.text = $"Lv1";
            _localizedName.RefreshString();
        }

        public void OnSelect(BaseEventData _)
        {
            InspectCharacterEvent?.Invoke(_cachedInfo);
        }
    }
}