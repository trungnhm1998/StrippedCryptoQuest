using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern.Sagas
{
    public class GetNftCharactersInParty : SagaBase<GetCharacters>
    {
        private IPartyController _partyController;

        private readonly List<ICharacterData> _inPartyCharacters = new();
        private readonly List<Obj.Character> _charactersInParty = new();
        private bool _isUsingMock = false;

        protected override void HandleAction(GetCharacters ctx)
        {
            if (_isUsingMock) return;
            InitMockData();
            ActionDispatcher.Dispatch(new GetInPartyNftCharactersSucceed(_charactersInParty));
        }

        private void InitMockData()
        {
            _inPartyCharacters.Clear();
            _partyController = ServiceProvider.GetService<IPartyController>();

            foreach (var character in _partyController.Slots)
            {
                if (character.IsValid() == false) continue;
                if (character.HeroBehaviour.Spec.Id == 0) continue; // if the hero is Abel then pass
                var obj = new CharacterData(character.HeroBehaviour, false);
                _inPartyCharacters.Add(obj);
            }

            _isUsingMock = true;
        }

    }
}