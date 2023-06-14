using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using CryptoQuest.SaveSystem;
using UnityEngine.Events;

namespace CryptoQuest.UI
{
    public class UITitleScreen : MonoBehaviour
    {
        #region Variables

        [Header("Entry Name")]
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _nameConfirmPromptPanel;
        [SerializeField] private Button _btnConfirm;
        [SerializeField] private Text _nameEntry;
        [SerializeField] private Text _confirmNameEntry;
        [SerializeField] private string _confirmMessage;

        public UnityAction ConfirmPromptAction;
        public UnityAction CancelPromptAction;
        public UnityAction StartGameAction;

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
            ConfirmPromptAction.Invoke();
        }

        public void CancelPrompt()
        {
            CancelPromptAction.Invoke();
            _nameEntryPanel.SetActive(_enablePanel);
            _nameConfirmPromptPanel.SetActive(!_enablePanel);
        }

        public void StartGame()
        {
            StartGameAction.Invoke();
        }

        #endregion
    }
}