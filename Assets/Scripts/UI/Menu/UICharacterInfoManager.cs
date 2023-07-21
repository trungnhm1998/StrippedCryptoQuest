using System.Collections;
using System.Collections.Generic;
using CryptoQuest.MockData;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UICharacterInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject _characterSlots;
        [SerializeField] private GameObject _characterInfoPF;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;

        private void Awake()
        {
            LoadPartyMembers();
        }

        private void LoadPartyMembers()
        {
            foreach (var item in _partyManagerMockData.Members)
            {
                var member = Instantiate(_characterInfoPF);
                member.GetComponentInChildren<UICharacterInfo>().CharInfoMockData = item;
                member.transform.parent = _characterSlots.transform;
            }
        }
    }
}
