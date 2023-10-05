using System;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private TMP_Text _goldUi;

        public void SetItemData(IEvolvableData equipment)
        {
            _icon.sprite = equipment.Icon;
            _nameLocalize.StringReference = equipment.LocalizedName;
            _goldUi.text = $"{equipment.Gold}G";

            Debug.Log($"icon = [{equipment.Icon}]");
        }
    }
}