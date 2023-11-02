using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIClassToChange : MonoBehaviour
    {
        public Action<UIOccupation> OnSelected;
        public Action<UIOccupation> OnSubmit;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _characterClassObject;

        private void OnItemPressed(UIOccupation item)
        {
            OnSubmit?.Invoke(item);
        }

        private void OnItemSelected(UIOccupation item)
        {
            OnSelected?.Invoke(item);
        }

        public void RenderClassToChange(List<CharacterClass> characterClasses)
        {
            CleanUpScrollView();
            foreach (var character in characterClasses)
            {
                var newClass = Instantiate(_characterClassObject, _scrollRect.content).GetComponent<UIOccupation>();
                newClass.OnSubmit += OnItemPressed;
                newClass.OnItemSelected += OnItemSelected;
                newClass.ConfigureCell(character);
            }
            StartCoroutine(SelectDefaultButton());
        }

        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            if (_scrollRect.content.childCount == 0) yield break;
            var firstItemGO = _scrollRect.content.GetChild(0).gameObject.GetComponent<UIOccupation>();
            EventSystem.current.SetSelectedGameObject(firstItemGO.gameObject);
            OnSelected?.Invoke(firstItemGO);
            OnSubmit?.Invoke(firstItemGO);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }

    }
}