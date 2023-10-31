using System;
using CryptoQuest.DimensionalBox.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.DimensionalBox.UI
{
    public class UIEquipment : MonoBehaviour
    {
        public event Action<UIEquipment> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;

        public void Initialize(NftEquipment equipment)
        {
            _nameText.text = "Item " + equipment.Id;
        }

        public void OnPressed()
        {
            Pressed?.Invoke(this);
        }
    }
}