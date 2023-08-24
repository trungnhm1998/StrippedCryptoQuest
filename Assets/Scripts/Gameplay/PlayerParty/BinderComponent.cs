using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    public class BinderComponent : MonoBehaviour, IPartyBinder
    {
        public event Action<IParty> Bound;

        [SerializeField] private PartyProviderBus _partyProviderBus;

        public void Bind(IParty party)
        {
            _partyProviderBus.Bind(party);
        }
    }
}