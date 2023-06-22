using System;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;

        private void Start()
        {
            _inputMediator.EnableMapGameplayInput();
        }

        private void OnEnable()
        {
            _inputMediator.InteractEvent += InteractEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.InteractEvent -= InteractEvent_Raised;
        }

        private void InteractEvent_Raised()
        {
            Debug.Log("Interact");
        }
    }
}