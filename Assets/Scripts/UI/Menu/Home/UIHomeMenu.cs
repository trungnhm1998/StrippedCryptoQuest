using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.MockData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Home
{
    public class UIHomeMenu : MonoBehaviour
    {
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        private void Awake()
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            for (int i = 0; i < _partyManagerMockData.Members.Count; i++)
            {
                ApplyData(i);
            }
        }

        private void ApplyData(int index)
        {
            _characterSlots.GetChild(index).GetComponentInChildren<UICharacterInfo>().CharInfoMockData = _partyManagerMockData.Members[index];
        }
    }
}
