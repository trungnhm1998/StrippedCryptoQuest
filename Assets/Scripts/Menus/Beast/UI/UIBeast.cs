using CryptoQuest.Character.Beast;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeast : MonoBehaviour, IBeastProvider
    {
        [SerializeField] private Image _beastIcon;
        [SerializeField] private MultiInputButton _beastButton;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private TMP_Text _beastNameText;
        [SerializeField] private Color _disableColor;
        [SerializeField] private ShowBeastDetailsTrigger _showBeastDetailsTrigger;

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

        public void Init(IBeast beast)
        {
            _beast = beast;
            _beastNameText.text = _beast.Name;
            _showBeastDetailsTrigger.Initialize(this);
        }

        public void OnPressButton() { }

        private void SetDisable(bool value)
        {
            _beastNameText.color = value ? _disableColor : _normalColor;
        }
    }
}