using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.PlayerParty;
using CryptoQuest.Input;
using DG.Tweening;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.SelectHero
{
    public class SelectHeroPresenter : MonoBehaviour
    {
        private const float SELECT_DELAY = 0.05f;
        public static event Action<HeroBehaviour> ConfirmSelectCharacter;

        [SerializeField] private BattleInput _battleInput;
        [SerializeField] private PlayerPartyPresenter _playerPartyPresenter;
        [SerializeField] private UISelectHeroButton _selectHeroButton;

        private int _currentIndex;
        private HeroBehaviour _inspectingCharacter;

        private void OnEnable()
        {
            _selectHeroButton.ConfirmPressed += OnConfirmCharacter;
            _battleInput.NavigateEvent += NavigateSelectHero;
        }

        private void OnDisable()
        {
            _selectHeroButton.ConfirmPressed -= OnConfirmCharacter;
            _battleInput.NavigateEvent -= NavigateSelectHero;
        }


        public void Show(LocalizedString str)
        {
            _selectHeroButton.SetLabel(str);
            SetCurrentCharacter(0);
            _selectHeroButton.SetUIActive(true);
            DOVirtual.DelayedCall(SELECT_DELAY, () =>
            {
                EventSystem.current.SetSelectedGameObject(_selectHeroButton.Button.gameObject);
            });
        }

        public void Hide()
        {
            _selectHeroButton.SetUIActive(false);
        }

        private void NavigateSelectHero(Vector2 direction)
        {
            if (direction.x == 0) return;
            bool isNavigateRight = direction.x > 0;
            int nextIndex = isNavigateRight ? _currentIndex + 1 : _currentIndex - 1;
            SetCurrentCharacter(nextIndex);
        }

        private void OnConfirmCharacter()
        {
            ConfirmSelectCharacter?.Invoke(_inspectingCharacter);
        }

        private void SetCurrentCharacter(int index)
        {
            if (!IsIndexValid(index)) return;
            _currentIndex = index;
            var characterUI = _playerPartyPresenter.CharacterUIs[index];
            if (characterUI.Hero == null) return;
            _inspectingCharacter = characterUI.Hero;
            _selectHeroButton.SetUIPosition(characterUI.transform.position);
        }

        private bool IsIndexValid(int index)
        {
            var characterUIs = _playerPartyPresenter.CharacterUIs;
            if (characterUIs.Length < 0) return false;
            return (0 <= index && index < _playerPartyPresenter.CharacterUIs.Length);
        }
    }
}