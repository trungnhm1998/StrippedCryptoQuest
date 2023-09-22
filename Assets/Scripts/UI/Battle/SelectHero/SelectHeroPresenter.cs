using UnityEngine;
using CryptoQuest.UI.Battle.StateMachine;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle.PlayerParty;
using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Battle.SelectHero
{
    public interface ISelectHeroModel
    {
        LocalizedString Label { get; }
    }

    public class SelectHeroPresenter : MonoBehaviour
    {
        public static event Action<CharacterSpec> ConfirmSelectCharacter;

        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private PlayerPartyPresenter _playerPartyPresenter;
        [SerializeField] private UISelectHeroButton _selectHeroButton;

        private int _currentIndex;
        private CharacterSpec _inspectingCharacter;

        private ISelectHeroModel _selectHeroModel;

        private void Awake()
        {
            _selectHeroModel = GetComponent<ISelectHeroModel>();
        }

        private void OnEnable()
        {
            SelectHeroState.EnteredState += EnterSelectHeroState;
            _selectHeroButton.ConfirmPressed += OnConfirmCharacter;
            _battleInput.NavigateEvent += NavigateSelectHero;
        }

        private void OnDisable()
        {
            SelectHeroState.EnteredState -= EnterSelectHeroState;
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
            _inspectingCharacter = characterUI.Member;
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