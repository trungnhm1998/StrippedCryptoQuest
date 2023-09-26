using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.PlayerParty;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.SelectHero
{
    public interface ISelectHeroModel
    {
        LocalizedString Label { get; }
    }

    public class SelectHeroPresenter : MonoBehaviour
    {
        public static event Action<HeroBehaviour> ConfirmSelectCharacter;

        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private PlayerPartyPresenter _playerPartyPresenter;
        [SerializeField] private UISelectHeroButton _selectHeroButton;

        private int _currentIndex;
        private HeroBehaviour _inspectingCharacter;
        private ISelectHeroModel _selectHeroModel;

        private void Awake()
        {
            _selectHeroModel = GetComponent<ISelectHeroModel>();
        }

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

        public void SetUILabel(ISelectHeroModel model)
        {
            _selectHeroButton.SetLabel(model?.Label);
        }

        private void EnterSelectHeroState()
        {
            SetCurrentCharacter(0);
            SetUILabel(_selectHeroModel);
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