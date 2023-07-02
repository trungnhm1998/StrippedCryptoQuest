using CryptoQuest.System.Settings;
using IndiGames.Core.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Title
{
    public class UINameInputPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [SerializeField] private TMP_InputField _playerNamePlaceholder;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;

        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private SaveSystemSO _saveSystemSO;

        private IStringValidator _nameValidator;

        public UnityAction OnConfirmButtonPressed;

        private void Awake()
        {
            _nameValidator = new NameValidator(_textAsset);
            _playerNamePlaceholder.ActivateInputField();
        }


        public bool IsValid()
        {
            var validation = _nameValidator.Validate(_playerNamePlaceholder.text);
            _validationStringEvent.StringReference.TableEntryReference = "TITLE_VALIDATE_" + (int)validation;

            if (validation != EValidation.Valid)
            {
                _validationText.color = Color.red;
                return false;
            }

            _saveSystemSO.PlayerName = _playerNamePlaceholder.text;
            _validationText.color = Color.white;
            return true;
        }

        public bool IsPlayerNameEmpty()
        {
            return string.IsNullOrEmpty(_saveSystemSO.PlayerName);
        }

        public void SetPlayerName(string value)
        {
            _saveSystemSO.PlayerName = value;
        }

        public void ConfirmButtonPressed()
        {
            OnConfirmButtonPressed?.Invoke();
        }

        public void Show() => _content.SetActive(true);

        public void Hide() => _content.SetActive(false);
    }
}