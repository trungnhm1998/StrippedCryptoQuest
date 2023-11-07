using System;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Sagas.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI
{
    public class UIEquipment : MonoBehaviour
    {
        public event Action<UIEquipment> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        
        public uint Id { get; private set; }

        public void Initialize(EquipmentResponse equipment)
        {
            Id = equipment.id;
            _nameText.text = "Item " + equipment.id;
            _equippedTag.SetActive(equipment.isEquipped);
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf) return;
            Pressed?.Invoke(this);
        }

        public void EnablePendingTag(bool enabling) => _pendingTag.SetActive(enabling);
    }
}