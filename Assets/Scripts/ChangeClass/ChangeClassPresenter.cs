using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.ScriptableObjects;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [field: SerializeField] public List<UIClassMaterial> ListClassMaterial { get; private set; }
        [SerializeField] private List<ConsumableSO> _itemMaterials;
        [SerializeField] private List<UIItemMaterial> _uiMaterials;
        [SerializeField] private List<ChangeClassSO> _listCharacterClass;
        [SerializeField] private UIClassCharacter _uiClassToChange;
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private WalletMaterialAPI _materialApi;
        [SerializeField] private WalletCharacterAPI _characterAPI;
        [SerializeField] private HeroInventorySO _heroInventorySO;
        private HeroInventorySO _changeClassHeroData;
        private IPartyController _partyController;
        public UIOccupation Occupation { get; private set; }
        private bool _isEmptyClassMaterial;
        public bool IsValid { get; private set; }

        private void OnEnable()
        {
            Init();
            _uiClassToChange.OnSelected += HandleSelectedOccupation;
        }

        private void OnDisable()
        {
            _uiClassToChange.OnSelected -= HandleSelectedOccupation;
        }

        public void Init()
        {
            GetAllHerosData();
            _syncData.ReleaseAllAssetReference();
            _isEmptyClassMaterial = false;
            StartCoroutine(LoadDataToChangeClass());
            EnableClassInteractable(true);
        }

        private void GetAllHerosData()
        {
            _changeClassHeroData = new();
            _partyController = ServiceProvider.GetService<IPartyController>();
            HeroInventorySO partyHeros = new();

            foreach (var data in _partyController.Slots)
            {
                if (data.IsValid() == false) break;
                partyHeros.OwnedHeroes.Add(data.Spec.Hero);
            }

            var listHeroInventory = _heroInventorySO.OwnedHeroes;
            var listHeroParty = partyHeros.OwnedHeroes;

            var combinedList = listHeroInventory.Concat(listHeroParty);

            var distinctList = combinedList.GroupBy(x => x.Id).Select(g => g.First()).ToList();
            _changeClassHeroData.OwnedHeroes = distinctList;
        }

        private IEnumerator LoadDataToChangeClass()
        {
            yield return null;
            _uiClassToChange.RenderClassToChange(_listCharacterClass);
            StartCoroutine(GetWalletItemMaterial());
            StartCoroutine(GetWalletCharacterMaterial());
        }

        private IEnumerator GetWalletItemMaterial()
        {
            _materialApi.LoadMaterialsFromWallet();
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);
            if (_materialApi.Data.Count <= 0) yield break;
            StartCoroutine(RenderItemMaterial());
        }

        private IEnumerator GetWalletCharacterMaterial()
        {
            _characterAPI.LoadCharacterFromWallet();
            yield return new WaitUntil(() => _characterAPI.IsFinishFetchData);
            if (_characterAPI.Data.Count <= 0) yield break;
            RenderClassMaterial();
            StartCoroutine(ValidateChangeClassMaterial());
        }

        private void HandleSelectedOccupation(UIOccupation occupation)
        {
            Occupation = occupation;
            RenderClassMaterial();
            StartCoroutine(RenderItemMaterial());
            StartCoroutine(ValidateChangeClassMaterial());
        }

        private void RenderClassMaterial()
        {
            if (Occupation == null) return;
            for (int i = 0; i < ListClassMaterial.Count; i++)
            {
                StartCoroutine(ListClassMaterial[i].InstantiateData(_changeClassHeroData.OwnedHeroes, Occupation, i));
            }
        }

        private IEnumerator RenderItemMaterial()
        {
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);

            foreach (var item in _itemMaterials)
            {
                if (item.ID == Occupation.Class.ItemMaterialId.ToString())
                {
                    _uiMaterials[0].SetLocalization(item.DisplayName);
                }
            }

            for (int index = 0; index < _materialApi.Data.Count; index++)
            {
                if (_materialApi.Data[index].materialId == Occupation.Class.ItemMaterialId.ToString())
                {
                    int quantity = Occupation.Class.MaterialQuantity;
                    foreach (var itemMaterial in _uiMaterials)
                    {
                        itemMaterial.ConfigureCell(_materialApi, quantity, index);
                    }
                    yield break;
                }
            }
        }

        private IEnumerator ValidateChangeClassMaterial()
        {
            IsValid = false;
            _isEmptyClassMaterial = false;
            foreach (var classMaterial in ListClassMaterial)
            {
                yield return new WaitUntil(() => classMaterial.IsFinishInstantiateData);
                if (classMaterial.IsEmptyMaterial)
                {
                    _isEmptyClassMaterial = classMaterial.IsEmptyMaterial;
                    break;
                }
            }

            bool isSameClass = ListClassMaterial[0].ClassID == ListClassMaterial[1].ClassID;
            bool isMaterialValid = !_isEmptyClassMaterial && _uiMaterials[0].IsValid;

            if (isSameClass && ListClassMaterial[0].ListClassCharacter.Count <= 1 || !isMaterialValid)
                yield break;

            IsValid = isMaterialValid;
            Occupation.EnableDefaultBackground(IsValid);
        }

        public void EnableClassInteractable(bool isEnable)
        {
            _uiClassToChange.EnableInteractable(isEnable);
        }

        public void SetSelectedClass(bool isEnable)
        {
            Occupation.EnableSelectedBackground(isEnable);
        }
    }
}