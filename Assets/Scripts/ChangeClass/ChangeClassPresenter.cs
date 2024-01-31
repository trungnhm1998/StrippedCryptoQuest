using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.ScriptableObjects;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIClassMaterial FirstClassMaterials { get; private set; }
        [field: SerializeField] public UIClassMaterial SecondClassMaterials { get; private set; }
        [SerializeField] private List<ConsumableSO> _itemMaterials;
        [SerializeField] private List<UIItemMaterial> _uiMaterials;
        [SerializeField] private List<ChangeClassSO> _listCharacterClass;
        [SerializeField] private UIClassCharacter _uiClassToChange;
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private WalletMaterialAPI _materialApi;
        [SerializeField] private WalletCharacterAPI _characterAPI;
        [SerializeField] private HeroInventorySO _heroInventorySO;
        [SerializeField] private ClassBerserkerController _classBerserkerController;
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
            EnableClassInteractable(true);
            StartCoroutine(LoadDataToChangeClass());
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
            StartCoroutine(_materialApi.LoadMaterialsFromWallet());
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);
            StartCoroutine(RenderItemMaterial());
        }

        private IEnumerator GetWalletCharacterMaterial()
        {
            StartCoroutine(_characterAPI.LoadCharacterFromWallet());
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
            _classBerserkerController.HandleSelectedOccupation(_changeClassHeroData, Occupation);
        }

        private void RenderClassMaterial()
        {
            if (Occupation == null) return;
            StartCoroutine(FirstClassMaterials.InstantiateData(_changeClassHeroData.OwnedHeroes, Occupation, 0));
            StartCoroutine(SecondClassMaterials.InstantiateData(_changeClassHeroData.OwnedHeroes, Occupation, 1));
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

            yield return new WaitUntil(() => SecondClassMaterials.IsFinishInstantiateData);

            _isEmptyClassMaterial = SecondClassMaterials.IsEmptyMaterial;


            if (IsNotValidSpecialClassMaterial() || !IsValidItemMaterial())
                yield break;

            IsValid = IsValidClassMaterial();
            yield return new WaitUntil(() => Occupation != null);
            Occupation.EnableDefaultBackground(IsValid);
        }

        private bool IsNotValidSpecialClassMaterial()
        {
            return FirstClassMaterials.ClassID == SecondClassMaterials.ClassID &&
                   FirstClassMaterials.ListClassCharacter.Count <= 1;
        }

        private bool IsValidItemMaterial()
        {
            return !_isEmptyClassMaterial && _uiMaterials[0].IsValid;
        }

        private bool IsValidClassMaterial()
        {
            return FirstClassMaterials.ListClassCharacter
                .Any(character1 => SecondClassMaterials.ListClassCharacter
                    .Any(character2 => character1.Class.Origin == character2.Class.Origin));
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