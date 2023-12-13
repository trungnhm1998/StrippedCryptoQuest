using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastListEvolve : MonoBehaviour
    {
        public event Action<UIBeastEvolve> OnSelectedEvent;
        public event Action<bool> IsOpenDetailsEvent;
        [SerializeField] private UIBeastEvolve _beast;
        [SerializeField] private ScrollRect _scrollRect;
        private UIBeastEvolve _baseMaterial;
        private IBeastModel _model;

        public void Init(IBeastModel model)
        {
            _model = model;
            CleanUpScrollView();
            InstantiateBeast(model);
        }

        private void InstantiateBeast(IBeastModel model)
        {
            foreach (var beast in model.Beasts.Where(b => b.Level >= b.MaxLevel))
            {
                var newBeast = Instantiate(_beast, _scrollRect.content);
                newBeast.OnBeastSelected += OnItemSelected;
                newBeast.ConfigureCell(beast);
            }

            IsOpenDetailsEvent?.Invoke(_scrollRect.content.childCount > 0);
            StartCoroutine(SelectDefaultButton(0));
        }

        public void FilterMaterial(UIBeastEvolve uiBeast)
        {
            CleanUpScrollView();
            var baseMaterial = Instantiate(_beast, _scrollRect.content);
            baseMaterial.ConfigureCell(uiBeast.Beast);
            baseMaterial.SetBaseMaterial();
            
            foreach (var model in _model.Beasts)
            {
                if (IsValidMaterial(uiBeast.Beast, model))
                {
                    var newBeast = Instantiate(_beast, _scrollRect.content);
                    newBeast.OnBeastSelected += OnItemSelected;
                    newBeast.ConfigureCell(model);
                }
            }

            StartCoroutine(SelectDefaultButton(1));
            baseMaterial.SetBaseMaterial();
        }

        private bool IsValidMaterial(IBeast beast, IBeast material)
        {
            return beast.Id != material.Id &&
                   beast.Type == material.Type &&
                   beast.Stars == material.Stars &&
                   beast.Class == material.Class &&
                   material.Level >= material.MaxLevel;
        }

        private void OnItemSelected(UIBeastEvolve beast)
        {
            OnSelectedEvent?.Invoke(beast);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator SelectDefaultButton(int index)
        {
            yield return null;
            if (_scrollRect.content.childCount <= index) yield break;
            var firstChild = _scrollRect.content.GetChild(index);
            EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
        }
    }
}