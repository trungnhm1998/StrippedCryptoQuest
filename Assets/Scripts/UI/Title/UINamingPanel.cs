using System.Collections;
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
        [field: SerializeField] public Button ConfirmButton { get; private set; }
        [field: SerializeField] public SaveSystemSO SaveSystemSO { get; private set; }
        [field: SerializeField] public TMP_InputField NameInput { get; private set; }
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private TextAsset _badWordAsset;
        [SerializeField] private TextAsset _specialCharacterAsset;
        [SerializeField] private SaveSystemSO _tempSaveInfo;
        private IStringValidator _nameValidator;

        public UnityAction ConfirmNameButtonPressed;
        public UnityAction CancelEvent;
        private bool _isInputValid;

        private const string VALIDATION_TABLE_ENTRY = "TITLE_VALIDATE_";

        private void Awake()
        {
            _tempSaveInfo.PlayerName = "";
            _nameValidator = new NameValidator(_badWordAsset, _specialCharacterAsset);
        }

        private void OnEnable()
        {
            NameInput.onSubmit.AddListener(MenuSubmitEvent);
            NameInput.Select();
            StartCoroutine(CoSelectNameInput());
        }

        private void OnDisable()
        {
            NameInput.onSubmit.RemoveListener(MenuSubmitEvent);
        }

        public void OnConfirmButtonPressed()
        {
            if (!_isInputValid)
            {
                NameInput.Select();
                return;
            }

            SaveSystemSO.PlayerName = NameInput.text;
            ConfirmNameButtonPressed?.Invoke();
        }

        public void ValidateInput(string input)
        {
            _isInputValid = IsNameValid(input);
        }

        public bool IsInputValid()
        {
            return _isInputValid;
        }

        private void HandleInputSubmit()
        {
            if (EventSystem.current.currentSelectedGameObject != NameInput.gameObject) return;
            MenuSubmitEvent(NameInput.text);
        }

        private void MenuSubmitEvent(string input)
        {
            if (IsNameValid(input))
                ConfirmButton.Select();
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

        public void SetTempName(string text)
        {
            _tempSaveInfo.PlayerName = text;
        }

        private void NavigateToNextInput()
        {
            var currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == NameInput.gameObject)
                ConfirmButton.Select();
            else
                NameInput.Select();
        }

        private IEnumerator CoSelectNameInput()
        {
            yield return new WaitForSeconds(.03f); // highlight event system bug workaround
            NameInput.Select();
        }
    }
}