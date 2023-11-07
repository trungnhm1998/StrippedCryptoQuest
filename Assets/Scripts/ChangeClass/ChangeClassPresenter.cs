using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.Interfaces;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [field: SerializeField] public List<UIClassMaterial> ListClassMaterial { get; private set; }
        [SerializeField] private List<CharacterClass> _listCharacterClass;
        [SerializeField] private UIClassToChange _uiClassToChange;
        [SerializeField] private UIItemMaterial _uiMaterial;
        [SerializeField] private WalletMaterialAPI _materialApi;
        private IWalletCharacterModel _characterModel;
        private List<ICharacterModel> _characterData = new();
        private UIOccupation _occupation;
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
            _characterModel = GetComponent<IWalletCharacterModel>();
            StartCoroutine(LoadDataToChangeClass());
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
            yield return _characterModel.CoGetData();
            yield return new WaitUntil(() => _characterModel.IsLoaded);
            if (_characterModel.Data.Count <= 0) yield break;
            _characterData = _characterModel.Data;
            RenderClassMaterial();
            StartCoroutine(ValidateChangeClassMaterial());
        }

        private void HandleSelectedOccupation(UIOccupation occupation)
        {
            _occupation = occupation;
            RenderClassMaterial();
            StartCoroutine(RenderItemMaterial());
            StartCoroutine(ValidateChangeClassMaterial());
        }

        private void RenderClassMaterial()
        {
            for (int i = 0; i < ListClassMaterial.Count; i++)
            {
                StartCoroutine(ListClassMaterial[i].InstantiateData(_characterData, _occupation, i));
            }
        }

        private IEnumerator RenderItemMaterial()
        {
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);
            for (int index = 0; index < _materialApi.Data.Count; index++)
            {
                if (_materialApi.Data[index].materialId == _occupation.Class.ItemMaterialId.ToString())
                {
                    int quantity = _occupation.Class.MaterialQuantity;
                    _uiMaterial.ConfigureCell(_materialApi, quantity, index);
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
                yield return new WaitUntil(() => classMaterial.IsFinishInstantiateData == true);
                if (classMaterial.IsEmptyMaterial)
                {
                    _isEmptyClassMaterial = classMaterial.IsEmptyMaterial;
                    break;
                }
            }
            if (_isEmptyClassMaterial || !_uiMaterial.IsValid) yield break;
            IsValid = true;
            _occupation.EnableDefaultBackground(IsValid);
        }
    }
}