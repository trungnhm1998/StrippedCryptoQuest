using System.Collections;
using CryptoQuest.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private EquipmentPrefabDatabase _database;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconNFT;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private RectTransform _tooltipPosition;

        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        [SerializeField] private bool _loadImageOnEnable;

        private EquipmentInfo _equipment;
        public EquipmentInfo Equipment => _equipment;
        private EquipmentPrefab _prefab;

        public void Init(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;
            _equipment = equipment;
            var data = _equipment.Data;
            _iconNFT.SetActive(_equipment.IsNft);
            _nameText.text = $"{data.ID}-{data.PrefabId}";
            _nameText.color = _enabledColor;
            StartCoroutine(CoLoadAndInitEquipment());
        }

        private IEnumerator CoLoadAndInitEquipment()
        {
            yield return _database.LoadDataById(_equipment.Data.PrefabId);
            _prefab = _database.GetDataById(_equipment.Data.PrefabId);
            if (!_prefab.DisplayName.IsEmpty) _nameLocalize.StringReference = _prefab.DisplayName;
            _icon.sprite = _prefab.EquipmentType.Icon;
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        public void Reset() => _equipment = default;

        private void OnDisable()
        {
            if (_equipment == null || _equipment.IsValid() == false) return;
            _database.ReleaseDataById(_equipment.Data.PrefabId);
        }
    }
}