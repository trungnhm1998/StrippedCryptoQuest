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
        // [SerializeField] private SaveMockup _saveMockup;


        [Header("Entry Name")]
        [SerializeField] private GameObject _entryPanel;
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _promptPanel;
        [SerializeField] private Button _nameEntryBtn;
        [SerializeField] private Text _nameEntryText;
        [SerializeField] private Text _confirmNameEntry;
        [SerializeField] private string _confirmMessage;
        #endregion


        #region Class
        private void CheckNameEntry()
        {
            _nameEntryBtn.interactable = _nameEntryText.text.Length != 0;
        }

        public void ConfirmNameEntry()
        {
            _nameEntryPanel.SetActive(false);
            _promptPanel.SetActive(true);
            _confirmNameEntry.text = $"Your name is {_nameEntryText.text}\n You can't change it when game starts,\n is that OK?";
        }

        public void ConfirmPrompt()
        {
            // _saveMockup.PlayerName = _nameEntryText.text;
            // _saveMockup.SaveData(false);
        }
        public void CancelPrompt()
        {
            _nameEntryPanel.SetActive(true);
            _promptPanel.SetActive(false);
        }

        public void StartGame()
        {
            // if (_saveMockup.HasCharacter)
            // {
            //     Debug.Log("Already has character");
            // }
            // else
            // {
            //     _entryPanel.SetActive(true);
            // }
        }
        #endregion
    }
}