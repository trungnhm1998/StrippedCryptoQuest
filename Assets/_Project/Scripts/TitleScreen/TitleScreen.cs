using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuestClient
{
    public class TitleScreen : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private SaveProfile _saveProfile;


        [Header("Entry Name")]
        [SerializeField] private GameObject _entryPanel;
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _promptPanel;
        [SerializeField] private Button _nameEntryBtn;
        [SerializeField] private Text _nameEntryText;
        [SerializeField] private Text _confirmNameEntry;
        [SerializeField] private bool _enablePanel;
        [SerializeField] private string _confirmMessage;
        #endregion


        #region Class
        public void CheckNameEntry()
        {
            _nameEntryBtn.interactable = _nameEntryText.text.Length != 0;
        }

        public void ConfirmNameEntry()
        {
            _nameEntryPanel.SetActive(!_enablePanel);
            _promptPanel.SetActive(_enablePanel);
            _confirmNameEntry.text = _confirmMessage + _nameEntryText.text;
        }

        public void ConfirmPrompt()
        {
            _saveProfile.PlayerName = _nameEntryText.text;
            _saveProfile.SaveData(_nameEntryText.text);
        }
        public void CancelPrompt()
        {
            _nameEntryPanel.SetActive(_enablePanel);
            _promptPanel.SetActive(!_enablePanel);
        }

        public void StartGame()
        {
            if (_saveProfile.ExistProfile)
            {
                _enablePanel = false;
            }
            else
            {
                _enablePanel = true;
                _entryPanel.SetActive(_enablePanel);
            }
        }
        #endregion
    }
}