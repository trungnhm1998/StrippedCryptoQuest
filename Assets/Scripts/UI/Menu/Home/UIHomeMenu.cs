using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.MockData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Home
{
    public class UIHomeMenu : MonoBehaviour
    {
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _characterInfoPF;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        private void Awake()
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            for (int i = 0; i < _partyManagerMockData.Members.Count; i++)
            {
                _characterSlots.GetChild(i).GetComponentInChildren<UICharacterInfo>().CharInfoMockData = _partyManagerMockData.Members[i];
            }
        }
    }
}
