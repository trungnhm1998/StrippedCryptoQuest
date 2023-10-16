using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class CharacterPresenter : MonoBehaviour
    {
        [SerializeField] private List<UIUpgradeCharacter> _listCharacter;
        [SerializeField] private UIUpgradeEquipment _upgradeEquipment;
        private IPartyController _partyController;

        private void OnEnable()
        {
            Init();
            _upgradeEquipment.OnSelected += OnItemSelected;
        }

        private void OnDisable()
        {
            _upgradeEquipment.OnSelected -= OnItemSelected;
        }

        private void Init()
        {
            _partyController = ServiceProvider.GetService<IPartyController>();
            DisableCharacterObjects();
            LoadCharacterDetail();
        }

        private void OnItemSelected(UIUpgradeItem item)
        {
            PreviewCharacterStats(item);
        }

        private void LoadCharacterDetail()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].gameObject.SetActive(true);
                _listCharacter[i].LoadCharacterDetail(_partyController.Slots[i].HeroBehaviour);
            }
        }

        private void DisableCharacterObjects()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].gameObject.SetActive(false);
            }
        }

        private void PreviewCharacterStats(UIUpgradeItem item)
        {
            if (item == null) return;
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].Preview(item.UpgradeEquipment.Equipment);
            }
        }
    }
}