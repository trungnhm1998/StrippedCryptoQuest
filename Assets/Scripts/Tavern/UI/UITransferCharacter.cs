using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Inventory;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Merchant;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Common;
using CryptoQuest.UI.Utilities;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.UI
{
    internal class UITransferCharacter : MonoBehaviour
    {
        public event Action Closed;

        [SerializeField] private HeroInventorySO _heroInventorySO;
        [SerializeField] private MerchantInput _merchantInput;
        [SerializeField] private UICharacterList _inGame;
        [SerializeField] private UICharacterList _inWallet;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Reset();
        }

        private void OnDisable()
        {
            UnRegisterEvents();
        }
        
        private void RegisterEvents()
        {
            _merchantInput.CancelEvent += Close;
            _merchantInput.ResetEvent += Reset;
            _merchantInput.ExecuteEvent += Transfer;
            _merchantInput.NavigateEvent += SwitchFocusPanel;
        }

        private void UnRegisterEvents()
        {
            _merchantInput.CancelEvent -= Close;
            _merchantInput.ResetEvent -= Reset;
            _merchantInput.ExecuteEvent -= Transfer;
            _merchantInput.NavigateEvent -= SwitchFocusPanel;
        }

        private void Reset()
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            StartCoroutine(CoFetchCharacters());
        }

        private IEnumerator CoFetchCharacters()
        {
            UnRegisterEvents();
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .Get<CharactersResponse>(CharacterAPI.CHARACTERS)
                .ToYieldInstruction();
            yield return op;
            ActionDispatcher.Dispatch(new ShowLoading(false));
            RegisterEvents();

            if (op.HasError)
            {
                Debug.Log($"<color=white>Tavern::Saga::FetchProfileCharacters::Error</color>:: {op.Error}");
                yield break;
            }

            var response = op.Result;
            var allCharacters = response.data.characters;
            InitializeList(allCharacters);
        }

        private void InitializeList(CryptoQuest.Sagas.Objects.Character[] allCharacters)
        {
            var inGameCharacters = new List<CryptoQuest.Sagas.Objects.Character>();
            var inWalletCharacters = new List<CryptoQuest.Sagas.Objects.Character>();

            for (var index = 0; index < allCharacters.Length; index++)
            {
                var character = allCharacters[index];
                if (character.isHero == 1) continue;
                if (character.inGameStatus == 2)
                    inGameCharacters.Add(character);
                else
                    inWalletCharacters.Add(character);
            }

            _inGame.Init(inGameCharacters);
            _inWallet.Init(inWalletCharacters);

            _inWallet.GetOrAddComponent<SelectFirstChildInList>().Select();
            _inGame.GetOrAddComponent<SelectFirstChildInList>().Select();
        }

        private void SwitchFocusPanel(Vector2 axis)
        {
            UICharacterList listToFocus = axis.x switch
            {
                > 0 => _inWallet,
                < 0 => _inGame,
                _ => null
            };

            if (listToFocus == null) return;
            var lastSelected = listToFocus.GetComponent<CacheButtonSelector>().LastSelected;
            if (lastSelected == null)
            {
                listToFocus.GetOrAddComponent<SelectFirstChildInList>().Select();
                return;
            }
            EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
        }
        
        private void Transfer()
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            UnRegisterEvents();
            var characterToRemove = _inGame.GetSelectedItems();
            var characterToAdd = _inWallet.GetSelectedItems();
            
            if (characterToRemove.Count == 0 && characterToAdd.Count == 0) return;
            
            int[] idsToRemove = new int[characterToRemove.Count];
            int[] idsToAdd = new int[characterToAdd.Count];

            for (var index = 0; index < characterToRemove.Count; index++)
            {
                var characterListItem = characterToRemove[index];
                idsToRemove[index] = characterListItem.Spec.Id;
                _heroInventorySO.Remove(characterListItem.Spec);
            }

            for (var index = 0; index < characterToAdd.Count; index++)
            {
                var characterListItem = characterToAdd[index];
                idsToAdd[index] = characterListItem.Spec.Id;
                _heroInventorySO.Add(characterListItem.Spec);
            }
            
            StartCoroutine(CoTransfer(idsToRemove, idsToAdd));
        }

        private IEnumerator CoTransfer(int[] idsToRemove, int[] idsToAdd)
        {
            var body = new Dictionary<string, int[]>()
            {
                { "game", idsToAdd },
                { "wallet", idsToRemove }
            };

            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(body)
                .Put<TransferResponse>(CharacterAPI.TRANSFER)
                .ToYieldInstruction();
            yield return op;
            RegisterEvents();
            
            if (op.HasError)
            {
                Debug.Log($"<color=white>Tavern::UITransferCharacter::TransferCharactersToBothSideFailed::Error</color>:: {op.Error}");
                yield break;
            }

            yield return CoFetchCharacters();
        }


        private void Close() => Closed?.Invoke();
    }
}