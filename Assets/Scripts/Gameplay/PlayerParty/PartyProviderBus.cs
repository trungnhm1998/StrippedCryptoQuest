using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public interface IPartyProvider
    {
        IParty Party { get; }
    }

    public interface IParty { }

    public interface IPartyBinder
    {
        public event Action<IParty> Bound;
        void Bind(IParty party);
    }

    public class PartyProviderBus : ScriptableObject, IPartyProvider, IPartyBinder
    {
        private IParty _party;
        public IParty Party => _party;

        public event Action<IParty> Bound;

        public void Bind(IParty party)
        {
            _party = party;
            Bound?.Invoke(party);
        }
    }
}