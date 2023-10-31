using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.Tavern.Models
{
    public class MockWalletCharacterModel : MonoBehaviour, IWalletCharacterModel
    {
        [SerializeField] private int _dataLength;
        [SerializeField] private Sprite _classIcon;
        [SerializeField] private LocalizedString[] _localizedNames = new LocalizedString[4];

        private List<IWalletCharacterData> _walletData;
        public List<IWalletCharacterData> Data => _walletData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _walletData = new List<IWalletCharacterData>();
            InitMockData();
        }

        private void InitMockData()
        {
            for (var i = 0; i < _dataLength; i++)
            {
                Random rand = new Random();
                LocalizedString name = _localizedNames[rand.Next(_localizedNames.Length - 1)];
                int level = rand.Next(1, 99);

                var obj = new WalletCharacterData(_classIcon, name, level);

                _walletData.Add(obj);
            }
        }
    }
}