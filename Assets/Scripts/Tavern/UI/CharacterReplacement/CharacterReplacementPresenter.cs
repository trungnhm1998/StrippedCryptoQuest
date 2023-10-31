using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class CharacterReplacementPresenter : MonoBehaviour
    {
        [SerializeField] private TavernInputManager _inputManager;
        [SerializeField] private TavernDialogsManager _dialogsManager;

        [SerializeField] private UICharacterReplacement _uiCharacterReplacement;
        [SerializeField] private UICharacterListGame _uiCharacterListGame;
        [SerializeField] private UICharacterListWallet _uiCharacterListWallet;

        [Header("Dialog Messages")]
        [SerializeField] private LocalizedString _confirmMessage;

        private IGameCharacterModel _gameModel;
        private IWalletCharacterModel _walletModel;

        private List<IGameCharacterData> _gameData = new();
        private List<IWalletCharacterData> _walletData = new();

        private void OnEnable()
        {
            _inputManager.ExecuteEvent += SendItemsRequested;

            StartCoroutine(GetIngameCharacters());
            StartCoroutine(GetWalletCharacters());
        }

        private void OnDisable()
        {
            _inputManager.ExecuteEvent -= SendItemsRequested;
        }

        private IEnumerator GetIngameCharacters()
        {
            _gameModel = GetComponentInChildren<IGameCharacterModel>();
            yield return _gameModel.CoGetData();

            if (_gameModel.Data.Count <= 0) yield break;

            _gameData = _gameModel.Data;
            _uiCharacterListGame.SetGameData(_gameData, true);
        }
        
        private IEnumerator GetWalletCharacters()
        {
            _walletModel = GetComponentInChildren<IWalletCharacterModel>();
            yield return _walletModel.CoGetData();

            if (_walletModel.Data.Count <= 0) yield break;

            _walletData = _walletModel.Data;
            _uiCharacterListWallet.SetWalletData(_walletData, _walletData.Count <= 0 ? true : false);
        }

        private void SendItemsRequested()
        {
            _dialogsManager.ChoiceDialog
                .SetMessage(_confirmMessage)
                .Show();
        }
    }
}