using System;
using System.Collections;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconNFT;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private GameObject[] _stoneSlots;

        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        
        [Header("Config")]
        [SerializeField] private AssetReferenceT<MagicStoneInventory> _inventoryAsset;

        private MagicStoneInventory _stoneInventory;

        private IEquipment _equipment;
        public IEquipment Equipment => _equipment;

        public void Init(IEquipment equipment)
        {
            if (equipment.IsValid() == false) return;
            _equipment = equipment;
            var data = _equipment.Data;
            _iconNFT.SetActive(_equipment.IsNft);
            _nameText.text = $"{data.ID}-{_equipment.Prefab.ID}";
            _nameText.color = _enabledColor;
            if (!equipment.Prefab.DisplayName.IsEmpty) _nameLocalize.StringReference = equipment.Prefab.DisplayName;
            _icon.sprite = equipment.Type.Icon;
        }

        public void InitStone(int[] attachStones)
        {
            StartCoroutine(CoLoad());

            if (attachStones.Length <= 0)
            {
                for (int i = 0; i < attachStones.Length; i++)
                    _stoneSlots[i].SetActive(true);

                return;
            }

            for (int i = 0; i < _stoneInventory.MagicStones.Count; i++)
            {
                for (int j = 0; j < attachStones.Length; j++)
                {
                    if (_stoneInventory.MagicStones[i].ID == attachStones[j])
                    {
                        
                    }
                }
            }
        }

        private IEnumerator CoLoad()
        {
            var inventoryHandle = _inventoryAsset.LoadAssetAsync();
            yield return inventoryHandle;
            _stoneInventory = inventoryHandle.Result;
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        public void Reset() => _equipment = default;

        private void OnDisable()
        {
            foreach (var slot in _stoneSlots)
                slot.SetActive(false);
        }
    }
}