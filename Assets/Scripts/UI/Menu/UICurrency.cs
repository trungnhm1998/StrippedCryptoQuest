using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UICurrency : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private VoidEventChannelSO _menuOpenedEvent;

        [Header("UI Components")]
        [SerializeField] private Text _gold;
        [SerializeField] private Text _soul;
        [SerializeField] private Text _metaD;

        private void Awake()
        {
            _menuOpenedEvent.EventRaised += UpdateCurrencyUI;
        }

        private void OnDestroy()
        {
            _menuOpenedEvent.EventRaised -= UpdateCurrencyUI;
        }

        private void UpdateCurrencyUI()
        {
            _gold.text = _wallet.Gold.Amount.ToString();
            _soul.text = _wallet.Soul.Amount.ToString();
            _metaD.text = _wallet.Diamond.Amount.ToString();
        }
    }
}