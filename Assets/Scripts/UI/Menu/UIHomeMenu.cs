using System.Collections;
using System.Collections.Generic;
using CryptoQuest.MockData;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIHomeMenu : MonoBehaviour
    {
        [SerializeField] private Transform _characterSlots;
        [SerializeField] private GameObject _characterInfoPF;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        private void Awake()
        {
            PrepareSlots();
            LoadPartyMembers();
        }

        private void PrepareSlots()
        {
            if (_characterSlots.childCount <= 0) return;

            foreach (Transform child in _characterSlots)
            {
                Destroy(child.gameObject);
            }
        }

        private void LoadPartyMembers()
        {
            foreach (var member in _partyManagerMockData.Members)
            {
                var memberGO = Instantiate(_characterInfoPF);
                memberGO.GetComponentInChildren<UICharacterInfo>().CharInfoMockData = member;
                memberGO.transform.parent = _characterSlots;
            }
        }
    }
}
