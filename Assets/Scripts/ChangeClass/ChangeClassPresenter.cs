using UnityEngine;
using CryptoQuest.ChangeClass.Interfaces;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Networking.Menu.DimensionBox;
using CryptoQuest.ChangeClass.API;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [SerializeField] private List<CharacterClass> _listCharacterClass;
        [SerializeField] private UIClassToChange _uiClassToChange;
        [SerializeField] private UIClassMaterial _firstClassMaterialPanel;
        [SerializeField] private UIClassMaterial _secondClassMaterialPanel;
        [SerializeField] private WalletMaterialAPI _materialApi;
        private IWalletCharacterModel _characterModel;
        private List<ICharacterModel> _characterData = new();
        private UIOccupation _material;

        private void OnEnable()
        {
            Init();
            _uiClassToChange.OnSelected += RenderCharacterMaterial;
        }

        private void Init()
        {
            _characterModel = GetComponent<IWalletCharacterModel>();
            StartCoroutine(LoadDataToChangeClass());
        }

        private IEnumerator LoadDataToChangeClass()
        {
            yield return null;
            _uiClassToChange.RenderClassToChange(_listCharacterClass);
            StartCoroutine(GetWalletItemMaterial());
        }

        private IEnumerator GetWalletItemMaterial()
        {
            _materialApi.LoadMaterialsFromWallet();
            yield return new WaitUntil(() => _materialApi.IsFinishFetchData);
            if (_materialApi.Data.Count <= 0) yield break;
            //TODO: Use Material to validate change class
        }
        
        private IEnumerator GetWalletCharacterMaterial()
        {
            yield return _characterModel.CoGetData();
            yield return new WaitUntil(() => _characterModel.IsLoaded);
            if (_characterModel.Data.Count <= 0) yield break;
            _characterData = _characterModel.Data;
            _firstClassMaterialPanel.InstantiateData(_characterData);
            _secondClassMaterialPanel.InstantiateData(_characterData);
        }

        private void RenderCharacterMaterial(UIOccupation material)
        {
            _material = material;
            StartCoroutine(GetWalletCharacterMaterial());
        }
    }
}