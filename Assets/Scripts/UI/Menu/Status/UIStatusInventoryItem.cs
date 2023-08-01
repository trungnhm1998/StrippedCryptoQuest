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
    public class UIStatusInventoryItem : MonoBehaviour, ICell
    {
        [Serializable]
        public class MockData
        {
            public LocalizedString Name;

            public MockData Clone()
            {
                return new MockData()
                {
                    Name = Name,
                };
            }
        }

        [SerializeField] LocalizeStringEvent _name;
        [SerializeField] Text _itemOrder;
        [SerializeField] private GameObject _selectEffect;
        [SerializeField] private AssetReferenceT<GameObject> _assetReference;
        [SerializeField] private Transform _unequipContainer;
        
        private GameObject _unequipSlot;

        public void Select()
        {
            _selectEffect.SetActive(true);
        }

        public void Deselect()
        {
            _selectEffect.SetActive(false);
        }

        public void Init(MockData mockData, int index)
        {
            if (index == 0)
            {
                if (_unequipSlot == null)
                    _assetReference.LoadAssetAsync<GameObject>().Completed += UIPrefabLoaded;
                else
                    _unequipSlot.SetActive(true);
            }
            else
            {
                if (_unequipSlot != null)
                {
                    _unequipSlot.SetActive(false);
                }
                _name.StringReference = mockData.Name;
            }

            _itemOrder.text = index.ToString();
        }

        private void UIPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            var unequipSlotGO = obj.Result;
            _unequipSlot = Instantiate(unequipSlotGO, _unequipContainer);
        }
    }
}