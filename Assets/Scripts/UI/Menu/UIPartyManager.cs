using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    /// <summary>
    /// To managing the Party with UI
    /// </summary>
    public class UIPartyManager : MonoBehaviour, IPartyManager, IPartyProvider
    {
        [SerializeField] private PartyProviderBus _partyProviderBus;

        private IParty _party;

        public IParty Party => _partyProviderBus.Party;
        public IPartySlot Slots { get; set; }

        private void Awake()
        {
            _partyProviderBus.Bound += SetParty;
        }

        private void OnDestroy()
        {
            _partyProviderBus.Bound -= SetParty;
        }

        private void SetParty(IParty party)
        {
            _party = party;
        }
    }
}