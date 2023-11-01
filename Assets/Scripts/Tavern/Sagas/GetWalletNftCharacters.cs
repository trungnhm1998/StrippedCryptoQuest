using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetWalletNftCharacters : SagaBase<NftCharacterAction>
    {
        [SerializeField] private int _dataLength;
        [SerializeField] private Sprite _classIcon;
        [SerializeField] private LocalizedString[] _localizedNames = new LocalizedString[4];

        private readonly List<IWalletCharacterData> _walletCharacters = new();

        protected override void HandleAction(NftCharacterAction ctx)
        {
            InitMockData();
            ActionDispatcher.Dispatch(new GetWalletNftCharactersSucceed(_walletCharacters));
        }

        private void InitMockData()
        {
            for (var i = 0; i < _dataLength; i++)
            {
                Random rand = new Random();
                LocalizedString name = _localizedNames[rand.Next(_localizedNames.Length - 1)];
                int level = rand.Next(1, 99);

                var obj = new WalletCharacterData(_classIcon, name, level);

                _walletCharacters.Add(obj);
            }
        }
    }
}