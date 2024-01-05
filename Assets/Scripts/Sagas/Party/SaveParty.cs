using System;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Sagas.Character;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas.Party
{
    public class SaveParty : SagaBase<SavePartyAction>
    {
        [SerializeField] private PartySO _party;

        private List<PartySlotSpec> _partySlotSpecs = new();

        protected override void HandleAction(SavePartyAction ctx)
        {
            _partySlotSpecs.Clear();
            var nonNftHeroSpec = new HeroSpec();
            for (int i = 0; i < _party.GetParty().Length; i++)
            {
                if (_party.GetParty()[i].Hero.Origin.DetailInformation.Id != 0) continue;
                nonNftHeroSpec = _party.GetParty()[i].Hero;
            }

            var converter = ServiceProvider.GetService<IHeroResponseConverter>();
            int nonNftHeroIndex = -1;
            for (int i = 0; i < ctx.Heroes.Length; i++)
            {
                if (ctx.Heroes[i].isHero == 1)
                {
                    _partySlotSpecs.Add(new PartySlotSpec() { Hero = nonNftHeroSpec });
                    nonNftHeroIndex = i;
                }
                else
                {
                    _partySlotSpecs.Add(new PartySlotSpec() { Hero = converter.Convert(ctx.Heroes[i])});
                }
            }
            _party.SetParty(_partySlotSpecs.ToArray());
            _party.GetParty()[nonNftHeroIndex].Hero.Id = ctx.Heroes[nonNftHeroIndex].id;
        }
    }
}