using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.ChangeClass.Interfaces;
using CryptoQuest.Networking.ChangeClass;
using UnityEngine;

namespace CryptoQuest.ChangeClass.Models
{
    [RequireComponent(typeof(WalletCharacterAPI))]
    public class WalletCharacterModel : MonoBehaviour, IWalletCharacterModel
    {
        private WalletCharacterAPI _walletCharacterAPI;

        public List<ICharacterModel> Data { get; private set; }
        public bool IsLoaded { get; private set; }

        private void Awake()
        {
            _walletCharacterAPI = GetComponent<WalletCharacterAPI>();
        }

        public IEnumerator CoGetData()
        {
            Data ??= new List<ICharacterModel>();
            Data.Clear();
            IsLoaded = false;
            _walletCharacterAPI.LoadCharacterFromWallet();
            yield return new WaitUntil(() => _walletCharacterAPI.IsFinishFetchData);

            if (_walletCharacterAPI.Data == null) yield break;
            StartCoroutine(ImportCharacter(_walletCharacterAPI.Data));
        }

        private IEnumerator ImportCharacter(List<CharacterAPI> characters)
        {
            yield return new WaitForSeconds(1f);
            foreach (var character in characters)
            {
                var obj = new CharacterData(character);
                Data.Add(obj);
            }
            IsLoaded = true;
        }
    }
}