using System;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenuInventoryItem : MonoBehaviour, ICell
    {
        [Serializable]
        public class Data
        {
            public LocalizedString Name;

            public Data Clone()
            {
                return new Data()
                {
                    Name = Name
                };
            }
        }

        [SerializeField] LocalizeStringEvent _name;
        [SerializeField] Text _itemOrder;
        [SerializeField] private GameObject _selectEffect;
        [SerializeField] private AssetReferenceT<GameObject> _assetReference;
        [SerializeField] private Transform _unequipContainer;

        private const string UNEQUIP_KEY = "ITEM_UNEQUIP";
        private const string ITEM_KEY = "ITEM_RUSTY_SWORD";
        private GameObject _unequipSlot;

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void Deselect()
        {
            _selectEffect.SetActive(false);
        }

        public void Init(Data data, int index)
        {
            if (index == 0)
            {
                if (_unequipSlot == null)
                    _assetReference.LoadAssetAsync<GameObject>().Completed += UIPrefabLoaded;
                else
                {
                    _name.SetEntry(UNEQUIP_KEY);
                    _unequipSlot.SetActive(true);
                }
            }
            else
            {
                if (_unequipSlot != null)
                {
                    _unequipSlot.SetActive(false);
                }
                _name.SetEntry(ITEM_KEY);
                _name.StringReference = data.Name;
                _itemOrder.text = index.ToString();
            }
        }

        private void UIPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            var unequipSlotGO = obj.Result;
            _unequipSlot = Instantiate(unequipSlotGO, _unequipContainer);
        }
    }
}