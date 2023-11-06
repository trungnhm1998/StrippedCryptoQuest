using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharactersInParty : SagaBase<GetCharacters>
    {
        private IPartyController _partyController;

        private readonly List<ICharacterData> _inPartyCharacters = new();
        private bool _isUsingMock = false;

        protected override void HandleAction(GetCharacters ctx)
        {
            if (_isUsingMock) return;
            InitMockData();
            ActionDispatcher.Dispatch(new GetInPartyNftCharactersSucceed(_inPartyCharacters));
        }

        private void InitMockData()
        {
            _inPartyCharacters.Clear();
            _partyController = ServiceProvider.GetService<IPartyController>();

            foreach (var character in _partyController.Slots)
            {
                if (character.HeroBehaviour.DisplayName == "Abel") continue; // temporary cheat
                var obj = new CharacterData(character.HeroBehaviour, false);
                _inPartyCharacters.Add(obj);
            }

            _isUsingMock = true;
        }

    }
}