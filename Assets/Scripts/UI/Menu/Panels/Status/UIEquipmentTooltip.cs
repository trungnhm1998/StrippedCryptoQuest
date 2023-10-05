using System;
using System.Collections;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Item;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIEquipmentTooltip : UITooltip
    {
        [SerializeField] private TMP_Text _effectDescription;
        [SerializeField] private Image _rarity;
        [SerializeField] private TMP_Text _level;
        public override ITooltip WithLevel(int equipmentLevel)
        {
            _level.text = $"Lv. { equipmentLevel}";
            return this;
        }

        public override ITooltip WithRarity(RaritySO rarity)
        {
            _rarity.sprite = rarity.Icon;
            return this;
        }
    }
}