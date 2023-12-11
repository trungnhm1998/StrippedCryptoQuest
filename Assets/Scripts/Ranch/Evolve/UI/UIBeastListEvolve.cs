using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Evolve.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastListEvolve : MonoBehaviour
    {
        public Action<UIBeastEvolve> OnSelected;
        [SerializeField] private UIBeastEvolve _beast;
        [SerializeField] private ScrollRect _scrollRect;

        public void Init(IBeastModel model)
        {
            CleanUpScrollView();
            foreach (var beast in model.Beasts)
            {
                var newBeast = Instantiate(_beast, _scrollRect.content);
                newBeast.OnBeastSelected += OnItemSelected;
                newBeast.ConfigureCell(beast);
            }
            StartCoroutine(SelectDefaultButton());
        }
        
        private void OnItemSelected(UIBeastEvolve beast)
        {
            OnSelected?.Invoke(beast);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }
        
        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            if (_scrollRect.content.childCount == 0) yield break;
            var firstChild = _scrollRect.content.GetChild(0);
            EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
        }
    }
}