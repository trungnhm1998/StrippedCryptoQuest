using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusInventory : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private PartyManagerMockDataSO _partyManagerMockData;
        
        private void Awake()
        {
            _inputMediator.EnableStatusMenuInput();
        }
        
        
    }
}
