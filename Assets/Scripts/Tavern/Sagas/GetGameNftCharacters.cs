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
    public class GetGameNftCharacters : SagaBase<GetCharacters>
    {
        [SerializeField] private int _dataLength;
        [SerializeField] private Sprite _classIcon;
        [SerializeField] private LocalizedString[] _localizedNames = new LocalizedString[4];

        private readonly List<ICharacterData> _inGameCharacters = new();
        private bool _isUsingMock = false;

        protected override void HandleAction(GetCharacters ctx)
        {
            if (_isUsingMock) return;
            InitMockGameData();
            ActionDispatcher.Dispatch(new GetGameNftCharactersSucceed(_inGameCharacters));
        }

        private void InitMockGameData()
        {
            _inGameCharacters.Clear();
            for (var i = 0; i < _dataLength; i++)
            {
                Random rand = new Random();
                LocalizedString localizedName = _localizedNames[rand.Next(_localizedNames.Length - 1)];
                int level = rand.Next(1, 99);

                var obj = new CharacterData(_classIcon, localizedName, level, false);

                _inGameCharacters.Add(obj);
            }

            _isUsingMock = true;
        }
    }
}