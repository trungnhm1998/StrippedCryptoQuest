using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusInventory : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _confirmSelectEquipmentSlotEvent;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        
        private void OnEnable()
        {
            _inputMediator.EnableStatusMenuInput();
            _confirmSelectEquipmentSlotEvent.EventRaised += ViewInventory;
        }

        private void OnDisable()
        {
            _inputMediator.EnableStatusMenuInput();
            _confirmSelectEquipmentSlotEvent.EventRaised -= ViewInventory;
        }

        private void ViewInventory()
        {
            _contents.SetActive(true);
        }
    }
}
