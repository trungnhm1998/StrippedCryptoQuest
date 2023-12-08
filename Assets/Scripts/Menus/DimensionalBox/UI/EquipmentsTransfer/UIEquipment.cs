using CryptoQuest.Sagas.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        public EquipmentResponse Equipment { get; private set; }
        public bool MarkedForTransfer
        {
            get => _pendingTag.activeSelf;
            set => _pendingTag.SetActive(value);
        }

        public uint Id => Equipment.id;

        public void Initialize(EquipmentResponse equipment)
        {
            MarkedForTransfer = false;
            Equipment = equipment;
            _nameText.text = "Item " + Id;
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf) return;
            MarkedForTransfer = !MarkedForTransfer;
        }
    }
}