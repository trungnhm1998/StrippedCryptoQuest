using System.Collections;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.System.Settings;
using IndiGames.Core.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINamingPanel : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private SaveSystemSO _saveSystemSO;

        [SerializeField] private Button _confirm;
        [SerializeField] private MenuSelectionHandler _selectionHandler;

        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private TextAsset _badWordAsset;
        [SerializeField] private TextAsset _specialCharacterAsset;

        private IStringValidator _nameValidator;

        public UnityAction ConfirmNameButtonPressed;
        public UnityAction CancelEvent;
        private bool _isInputValid;

        private const string VALIDATION_TABLE_ENTRY = "TITLE_VALIDATE_";

        private void Awake()
        {
            _nameValidator = new NameValidator(_badWordAsset, _specialCharacterAsset);
        }

        private void OnEnable()
        {
            _inputMediator.CancelEvent += BackToStartScreen;
            _inputMediator.MenuTabPressed += NavigateToNextInput;
            _inputMediator.MenuConfirmedEvent += HandleInputSubmit;
            _nameInput.onSubmit.AddListener(MenuSubmitEvent);

            _selectionHandler.UpdateDefault(_nameInput.gameObject);
            StartCoroutine(CoSelectNameInput());
        }

        private void OnDisable()
        {
            _selectionHandler.Unselect();
            _inputMediator.CancelEvent -= BackToStartScreen;
            _inputMediator.MenuTabPressed -= NavigateToNextInput;
            _inputMediator.MenuConfirmedEvent -= HandleInputSubmit;
            _nameInput.onSubmit.RemoveListener(MenuSubmitEvent);
        }

        public void OnConfirmButtonPressed()
        {
            if (!_isInputValid)
            {
                _nameInput.Select();
                return;
            }

            _saveSystemSO.PlayerName = _nameInput.text;
            ConfirmNameButtonPressed?.Invoke();
        }

        public void ValidateInput(string input)
        {
            _isInputValid = IsNameValid(input);
        }

        private void HandleInputSubmit()
        {
            if (EventSystem.current.currentSelectedGameObject != _nameInput.gameObject) return;
            MenuSubmitEvent(_nameInput.text);
        }

        private void MenuSubmitEvent(string input)
        {
            if (IsNameValid(input))
                _confirm.Select();
        }

        private void BackToStartScreen()
        {
            CancelEvent?.Invoke();
        }

        private bool IsNameValid(string input)
        {
            var validationCode = _nameValidator.Validate(input);
            _validationStringEvent.StringReference.TableEntryReference = VALIDATION_TABLE_ENTRY + (int)validationCode;

            var isValid = validationCode == EValidation.Valid;
            _validationText.color = isValid ? _validColor : _invalidColor;
            return isValid;
        }

        private void NavigateToNextInput()
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == _nameInput.gameObject)
                _confirm.Select();
            else
                _nameInput.Select();
        }

        public void OnEndEditInput()
        {
            _inputMediator.EnableMenuInput();
        }

        private IEnumerator CoSelectNameInput()
        {
            _inputMediator.DisableAllInput();
            yield return new WaitForSeconds(.03f); // highlight event system bug workaround
            _nameInput.Select();
        }
    }
}