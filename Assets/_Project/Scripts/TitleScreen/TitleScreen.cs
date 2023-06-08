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
        [SerializeField] private SaveProfile _saveProfile = new SaveProfile();
        [SerializeField] private ProfileScriptableObject _profileSO;

        [Header("Entry Name")]
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _promptPanel;
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Text _nameEntry;
        [SerializeField] private Text _confirmNameEntry;
        [SerializeField] private bool _enablePanel;
        [SerializeField] private string _confirmMessage;
        [SerializeField] private string _fileName;
        #endregion

        #region Unity_Method
        void Awake()
        {
            _saveProfile._profileSO = _profileSO;
            _saveProfile._fileName = _fileName;
        }
        #endregion


        #region Class
        public void CheckNameEntry()
        {
            _btnConfirm.interactable = _nameEntry.text.Length != 0;
        }

        public void ConfirmNameEntry()
        {
            _nameEntryPanel.SetActive(!_enablePanel);
            _promptPanel.SetActive(_enablePanel);
            _confirmNameEntry.text = _confirmMessage + _nameEntry.text;
        }

        public void ConfirmPrompt()
        {
            _saveProfile.PlayerName = _nameEntry.text;
            _saveProfile.SaveData();
        }
        public void CancelPrompt()
        {
            _nameEntryPanel.SetActive(_enablePanel);
            _promptPanel.SetActive(!_enablePanel);
        }

        public void StartGame()
        {
            _saveProfile.LoadData();
            _enablePanel = !_saveProfile.ExistProfile;
            _nameEntryPanel.SetActive(_enablePanel);
        }
        #endregion
    }
}