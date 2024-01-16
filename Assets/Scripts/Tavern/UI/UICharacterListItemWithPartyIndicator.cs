using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterListItemWithPartyIndicator : UICharacterListItem
    {
        [SerializeField] private GameObject _partyIndicator;

        public void MarkAsInParty(bool isInParty = true) => _partyIndicator.SetActive(isInParty);
        public bool IsMarkedAsInParty() => _partyIndicator.activeSelf;
    }
}