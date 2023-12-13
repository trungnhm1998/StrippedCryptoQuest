using System.Collections;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
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
        [SerializeField] private GameObject _transferringTag;
        [SerializeField] private RectTransform _contentRectTransform;

        private IEquipment _equipment;
        public IEquipment Equipment => _equipment;
        public EquipmentResponse Response { get; private set; }

        public bool MarkedForTransfer
        {
            get => _pendingTag.activeSelf;
            set => _pendingTag.SetActive(value);
        }

        public int Id => Response.id;

        public void Initialize(EquipmentResponse equipment)
        {
            _equipment = null;
            MarkedForTransfer = false;
            Response = equipment;
            _nameText.text = "Item " + Id;
            _equippedTag.SetActive(Response.IsEquipped);
            _transferringTag.SetActive(Response.IsTransferring || Response.mintStatus != 2);

            _equipment = ServiceProvider.GetService<IEquipmentResponseConverter>().Convert(equipment);

            StartCoroutine(UpdateUIWhenEquipmentLoaded()); // TODO: Potential unload
        }

        private IEnumerator UpdateUIWhenEquipmentLoaded()
        {
            yield return new WaitUntil(() => _equipment.IsValid());
            if (!_equipment.DisplayName.IsEmpty) _name.StringReference = _equipment.DisplayName;
            _icon.sprite = _equipment.Type.Icon;
            yield return new WaitForSeconds(0.5f);
            LayoutRebuilder.MarkLayoutForRebuild(_contentRectTransform);
        }

        private void OnDisable()
        {
            _equipment = null;
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf || _transferringTag.activeSelf) return;
            MarkedForTransfer = !MarkedForTransfer;
        }
    }
}