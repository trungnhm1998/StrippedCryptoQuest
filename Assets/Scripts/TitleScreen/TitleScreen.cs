using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuestClient
{
    public class TitleScreen : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private SaveManagerSO _saveManagerSO;
        [SerializeField] private ProfileScriptableObject _profileSO;

        [Header("Entry Name")]
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _nameConfirmPromptPanel;
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Text _nameEntry;
        [SerializeField] private Text _confirmNameEntry;
        [SerializeField] private string _confirmMessage;
        [SerializeField] private string _fileName;
        private bool _enablePanel;
        #endregion
        
        #region Class
        public void CheckNameEntry()
        {
            _btnConfirm.interactable = _nameEntry.text.Length != 0;
        }

        public void ConfirmNameEntry()
        {
            _nameEntryPanel.SetActive(!_enablePanel);
            _nameConfirmPromptPanel.SetActive(_enablePanel);
            _confirmNameEntry.text = _confirmMessage + _nameEntry.text;
        }

        public void ConfirmPrompt()
        {
            _profileSO.PlayerName = _nameEntry.text;
            _saveManagerSO.SaveData();
        }
        public void CancelPrompt()
        {
            _nameEntryPanel.SetActive(_enablePanel);
            _nameConfirmPromptPanel.SetActive(!_enablePanel);
        }

        public void StartGame()
        {
            _saveManagerSO.LoadData();
            _enablePanel = _profileSO.PlayerName == null;
            _nameEntryPanel.SetActive(_enablePanel);
        }
        #endregion
    }
}