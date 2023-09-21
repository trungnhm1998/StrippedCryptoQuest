using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Character
{
    public class UICharacterPartySlot : MonoBehaviour
    {
        [SerializeField] private Image _selectBorder;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private UICharacterInfoPanel _characterInSlot;

        public bool Interactable
        {
            get => _button.enabled;
            set
            {
                _button.enabled = value;
                _selectBorder.enabled = false;
            }
        }

        private int _indexInParty;

        public void Init(CharacterSpec member, int idxInParty)
        {
            _characterInSlot.gameObject.SetActive(member.IsValid());
            if (member.IsValid() == false) return;

            _indexInParty = idxInParty;
            _characterInSlot.Init(member);
            _button.onClick.AddListener(OnPressed);
            _button.Selected += Select;
            _button.DeSelected += Deselect;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnPressed);
            _button.Selected -= Select;
            _button.DeSelected -= Deselect;
        }

        private void OnPressed() => _onHeroSelected?.Invoke(_indexInParty);

        public void Select()
        {
            if (_characterInSlot.gameObject.activeSelf == false) return;
            _selectBorder.enabled = true;
        }

        private void Deselect()
        {
            _selectBorder.enabled = false;
        }

        private Action<int> _onHeroSelected;

        public void SetSelectedCallback(Action<int> onHeroSelected)
        {
            _onHeroSelected = onHeroSelected;
        }
    }
}