using System;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Menu;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeast : MonoBehaviour, IBeastProvider, ISelectHandler
    {
        public static Action<UIBeast> OnBeastSelected;

        [Header("UI")] [SerializeField] private Image _beastIcon;

        [SerializeField] private MultiInputButton _beastButton;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private TMP_Text _beastNameText;
        [SerializeField] private GameObject _equippedTag;
        [SerializeField] private Color _disableColor;

        [Header("Events")] [SerializeField] private ShowBeastDetailsTrigger _showBeastDetailsTrigger;

        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;

        public IBeast Beast => _beast;
        private IBeast _beast = NullBeast.Instance;

        public bool Interactable
        {
            set => _beastButton.interactable = value;
        }

        public bool MarkedForEquipped
        {
            get => _equippedTag.activeSelf;
            set => _equippedTag.SetActive(value);
        }

        public void Init(IBeast beast)
        {
            _beast = beast;

            _beastName.StringReference = _beast.LocalizedName;
            _beastIcon.sprite = _beast.Elemental.Icon;
            _showBeastDetailsTrigger.Initialize(this);
        }

        public void OnPressButton()
        {
            OnBeastSelected?.Invoke(this);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _calculatorBeastStatsSo.RaiseEvent(_beast);
        }
    }
}