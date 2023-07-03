using System.Collections;
using CryptoQuest.Input;
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
        [SerializeField] private Button _confirm;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;

        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private SaveSystemSO _saveSystemSO;

        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        private IStringValidator _nameValidator;

        public UnityAction ConfirmNameButtonPressed;
        public UnityAction CancelEvent;

        private const string VALIDATION_TABLE_ENTRY = "TITLE_VALIDATE_";

        private void Awake()
        {
            _nameValidator = new NameValidator(_textAsset);
        }

        private void OnEnable()
        {
            _inputMediator.CancelEvent += BackToStartScreen;
            _inputMediator.MenuTabPressed += NavigateToNextInput;
            _confirm.interactable = false;

            StartCoroutine(CoSelectNameInput());
        }

        private void OnDisable()
        {
            _inputMediator.CancelEvent -= BackToStartScreen;
            _inputMediator.MenuTabPressed -= NavigateToNextInput;
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

        public void ValidateInput(string input)
        {
            var isNameValid = IsNameValid(input);
            _confirm.interactable = isNameValid;

            if (!isNameValid)
            {
                StartCoroutine(CoSelectNameInput());
            }
        }

        public void HandleEndEdit(string input)
        {
            ValidateInput(input);

            NavigateToNextInput();
        }

        private void NavigateToNextInput()
        {
            if (!gameObject.activeSelf) return;

            var currentSelected = EventSystem.current.currentSelectedGameObject;

            StartCoroutine(currentSelected == _nameInput.gameObject
                ? CoSelectConfirmButton()
                : CoSelectNameInput());
        }

        private IEnumerator CoSelectConfirmButton()
        {
            yield return new WaitForSeconds(.03f); // highlight event system bug workaround
            _confirm.Select();
        }

        private IEnumerator CoSelectNameInput()
        {
            yield return new WaitForSeconds(.03f); // highlight event system bug workaround
            _nameInput.Select();
        }

        public void OnConfirmButtonPressed()
        {
            _saveSystemSO.PlayerName = _nameInput.text;
            ConfirmNameButtonPressed?.Invoke();
        }
    }
}