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

        [Header("Dialog Messages")]
        [SerializeField] private LocalizedString _confirmMessage;

        private IGameCharacterModel _gameModel;
        private IWalletCharacterModel _walletModel;

        private List<IGameCharacterData> _gameData = new();
        private List<IWalletCharacterData> _walletData = new();

        private void OnEnable()
        {
            _inputManager.ExecuteEvent += SendItemsRequested;

            StartCoroutine(GetGameEquipments());
        }

        private void OnDisable()
        {
            _inputManager.ExecuteEvent -= SendItemsRequested;
        }

        private IEnumerator GetGameEquipments()
        {
            _gameModel = GetComponentInChildren<IGameCharacterModel>();
            yield return _gameModel.CoGetData();

            if (_gameModel.Data.Count <= 0) yield break;

            _gameData = _gameModel.Data;
            _uiCharacterListGame.SetGameData(_gameData, true);
        }

        private void SendItemsRequested()
        {
            _dialogsManager.ChoiceDialog
                .SetMessage(_confirmMessage)
                .Show();
        }
    }
}