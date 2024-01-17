using System;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UITransferable : MonoBehaviour
    {
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _transferringTag;

        private void Awake()
        {
            _pendingTag.SetActive(false);
            _transferringTag.SetActive(false);
        }

        public bool IsPending
        {
            get => _pendingTag.activeSelf;
            set => _pendingTag.SetActive(value);
        }

        public bool IsTransferring
        {
            get => _transferringTag.activeSelf;
            set => _transferringTag.SetActive(value);
        }
        
        public void TogglePending() => IsPending = !IsPending;
    }
}