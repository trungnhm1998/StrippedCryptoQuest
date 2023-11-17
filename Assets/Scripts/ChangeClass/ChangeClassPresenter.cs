using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.ScriptableObjects;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [field: SerializeField] public List<UIClassMaterial> ListClassMaterial { get; private set; }
        [SerializeField] private List<UIItemMaterial> _listItemMaterial;
        [SerializeField] private List<ChangeClassSO> _listCharacterClass;
        [SerializeField] private UIClassCharacter _uiClassToChange;
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private WalletMaterialAPI _materialApi;
        [SerializeField] private WalletCharacterAPI _characterAPI;
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
            _isEmptyClassMaterial = false;
            StartCoroutine(LoadDataToChangeClass());
            EnableClassInteractable(true);
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
            _syncData.ReleaseAllAssetReference();
            RenderClassMaterial();
            StartCoroutine(RenderItemMaterial());
            StartCoroutine(ValidateChangeClassMaterial());
        }

        private void RenderClassMaterial()
        {
            if (Occupation == null) return;
            for (int i = 0; i < ListClassMaterial.Count; i++)
            {
                StartCoroutine(ListClassMaterial[i].InstantiateData(_characterAPI.Data, Occupation, i));
            }
        }

        private IEnumerator RenderItemMaterial()
        {
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);
            for (int index = 0; index < _materialApi.Data.Count; index++)
            {
                if (_materialApi.Data[index].materialId == Occupation.Class.ItemMaterialId.ToString())
                {
                    int quantity = Occupation.Class.MaterialQuantity;
                    foreach (var itemMaterial in _listItemMaterial)
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
            bool isMaterialValid = !_isEmptyClassMaterial && _listItemMaterial[0].IsValid;

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