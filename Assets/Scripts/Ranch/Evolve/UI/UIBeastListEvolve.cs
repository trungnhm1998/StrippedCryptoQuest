using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Beast;
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
        private List<EvolvableBeast> _evolvableBeasts;

        public void Init(List<EvolvableBeast> evolvableBeasts)
        {
            _evolvableBeasts = evolvableBeasts;
            CleanUpScrollView();
            InstantiateBeast();
        }

        private void InstantiateBeast()
        {
            foreach (var evolvable in _evolvableBeasts.Where(b => b.Beast.Level >= b.Beast.MaxLevel))
            {
                var newBeast = Instantiate(_beast, _scrollRect.content);
                newBeast.OnBeastSelected += OnItemSelected;
                newBeast.ConfigureCell(evolvable.Beast);
                newBeast.SetupCurrencyValue(evolvable);
            }

            IsOpenDetailsEvent?.Invoke(_scrollRect.content.childCount > 0);
            StartCoroutine(SelectDefaultButton(0));
        }

        public void FilterMaterial(UIBeastEvolve uiBeast)
        {
            CleanUpScrollView();
            var matchingEvolvable = _evolvableBeasts.FirstOrDefault(evolvable => evolvable.Beast == uiBeast.Beast);
            if (matchingEvolvable != null)
            {
                var baseMaterial = Instantiate(_beast, _scrollRect.content);
                baseMaterial.ConfigureCell(uiBeast.Beast);
                baseMaterial.SetBaseMaterial();
                baseMaterial.SetupCurrencyValue(matchingEvolvable);
                baseMaterial.SetBaseMaterial();
            }

            foreach (var evolvable in _evolvableBeasts)
            {
                if (IsValidMaterial(uiBeast.Beast, evolvable.Beast))
                {
                    var newBeast = Instantiate(_beast, _scrollRect.content);
                    newBeast.OnBeastSelected += OnItemSelected;
                    newBeast.ConfigureCell(evolvable.Beast);
                    newBeast.SetupCurrencyValue(evolvable);
                }
            }

            StartCoroutine(SelectDefaultButton(1));
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