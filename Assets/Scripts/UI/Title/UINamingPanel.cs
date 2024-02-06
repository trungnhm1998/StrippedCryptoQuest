using CryptoQuest.SaveSystem;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Title
{
    public class UINamingPanel : MonoBehaviour
    {
        [field: SerializeField] public Button ConfirmButton { get; private set; }
        [field: SerializeField] public TMP_InputField NameInput { get; private set; }
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private TextAsset _badWordAsset;
        [SerializeField] private TextAsset _specialCharacterAsset;
        [SerializeField] private SaveSystemSO _saveSystem;
        private IStringValidator _nameValidator;

        private bool _isInputValid;

        private const string VALIDATION_TABLE_ENTRY = "TITLE_VALIDATE_";

        private void Awake()
        {
            _nameValidator = new NameValidator(_badWordAsset, _specialCharacterAsset);
        }

        private void OnEnable()
        {
            ConfirmButton.interactable = false;
            NameInput.onSubmit.AddListener(MenuSubmitEvent);
            Invoke(nameof(SelectInputField), 0);
        }

        private void OnDisable() => NameInput.onSubmit.RemoveListener(MenuSubmitEvent);

        private void SelectInputField() => NameInput.Select();

        public void OnConfirmButtonPressed()
        {
            if (!_isInputValid)
            {
                NameInput.Select();
                return;
            }

            _saveSystem.PlayerName = NameInput.text;
            _saveSystem.Save();
        }

        public void ValidateInput(string input)
        {
            _isInputValid = IsNameValid(input);
            ConfirmButton.interactable = _isInputValid;
        }

        private void MenuSubmitEvent(string input)
        {
            if (IsNameValid(input))
                ConfirmButton.Select();
        }

        private bool IsNameValid(string input)
        {
            var validationCode = _nameValidator.Validate(input);
            _validationStringEvent.StringReference.TableEntryReference = VALIDATION_TABLE_ENTRY + (int)validationCode;

            var isValid = validationCode == EValidation.Valid;
            _validationText.color = isValid ? _validColor : _invalidColor;
            return isValid;
        }
    }
}