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
        [SerializeField] private Button _confirm;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private MenuSelectionHandler _selectionHandler;

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

            _selectionHandler.UpdateDefault(_nameInput.gameObject);
            StartCoroutine(CoSelectNameInput());
        }

        private void OnDisable()
        {
            _selectionHandler.Unselect();
            _inputMediator.CancelEvent -= BackToStartScreen;
            _inputMediator.MenuTabPressed -= NavigateToNextInput;
        }

        public void OnEndEdit()
        {
            _confirm.Select();
        }

        public void OnConfirmButtonPressed()
        {
            if (!IsNameValid(_nameInput.text))
            {
                _nameInput.Select();
                return;
            }

            _saveSystemSO.PlayerName = _nameInput.text;
            ConfirmNameButtonPressed?.Invoke();
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

        private IEnumerator CoSelectNameInput()
        {
            yield return new WaitForSeconds(.03f); // highlight event system bug workaround
            _nameInput.Select();
        }
    }
}