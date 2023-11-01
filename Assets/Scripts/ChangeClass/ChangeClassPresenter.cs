using UnityEngine;
using CryptoQuest.ChangeClass.Interfaces;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Networking.Menu.DimensionBox;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPresenter : MonoBehaviour
    {
        [SerializeField] private List<CharacterClass> _listCharacterClass;
        [SerializeField] private UIClassToChange _uiClassToChange;
        [SerializeField] private UIClassMaterial _firstClassMaterialPanel;
        [SerializeField] private UIClassMaterial _secondClassMaterialPanel;
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
            StartCoroutine(LoadClassToChange());
        }

        private IEnumerator LoadClassToChange()
        {
            yield return null;
            _uiClassToChange.RenderClassToChange(_listCharacterClass);
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