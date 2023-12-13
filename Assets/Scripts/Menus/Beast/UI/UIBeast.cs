using System;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Menu;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeast : MonoBehaviour, IBeastProvider, ISelectHandler
    {
        public static Action<UIBeast> OnBeastSelected;

        [Header("UI")]
        [SerializeField] private Image _beastIcon;

        [SerializeField] private MultiInputButton _beastButton;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private TMP_Text _beastNameText;
        [SerializeField] private GameObject _equippedTag;
        [SerializeField] private Color _disableColor;

        [Header("Events")]
        [SerializeField] private ShowBeastDetailsTrigger _showBeastDetailsTrigger;

        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;

        private Color _normalColor;
        private IBeast _beast;

        public IBeast Beast => _beast;

        public bool Interactable
        {
            set => _beastButton.interactable = value;
        }

        private void Awake()
        {
            _normalColor = _beastNameText.color;
        }

        public void EnableEquippedTag(bool value) => _equippedTag.SetActive(value);

        public void Init(IBeast beast)
        {
            _beast = beast;
            _beastNameText.text = _beast.Name;
            _showBeastDetailsTrigger.Initialize(this);
        }

        public void OnPressButton()
        {
            OnBeastSelected?.Invoke(this);
        }

        private void SetDisable(bool value)
        {
            _beastNameText.color = value ? _disableColor : _normalColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _calculatorBeastStatsSo.RaiseEvent(_beast);
        }
    }
}