using System;
using CryptoQuest.Sagas.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class UIEquipment : MonoBehaviour
    {
        public event Action<UIEquipment> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        public EquipmentResponse Equipment { get; private set; }
        public GameObject EquippedTag => _equippedTag;
        
        public uint Id { get; private set; }

        public void Initialize(EquipmentResponse equipment)
        {
            Equipment = equipment;
            Id = equipment.id;
            _nameText.text = "Item " + equipment.id;
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf) return;
            Pressed?.Invoke(this);
        }

        public void EnablePendingTag(bool enabling) => _pendingTag.SetActive(enabling);
    }
}