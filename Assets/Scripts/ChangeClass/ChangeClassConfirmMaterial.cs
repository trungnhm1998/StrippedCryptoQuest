using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassConfirmMaterial : MonoBehaviour
    {
        [SerializeField] private List<UIClassMaterial> _listClassMaterial;
        public UnityAction<UICharacter> FirstClassMaterialEvent;
        public UnityAction<UICharacter> LastClassMaterialEvent;
        private UICharacter _firstClassMaterial;
        private UICharacter _lastClassMaterial;

        private void OnEnable()
        {
            FirstClassMaterialEvent += GetFirstClassMaterial;
            LastClassMaterialEvent += GetLastClassMaterial;
        }

        private void OnDisable()
        {
            FirstClassMaterialEvent -= GetFirstClassMaterial;
            LastClassMaterialEvent -= GetLastClassMaterial;
        }

        private void GetFirstClassMaterial(UICharacter character)
        {
            _firstClassMaterial = character;
        }

        private void GetLastClassMaterial(UICharacter character)
        {
            _lastClassMaterial = character;
        }

        public void FilterClassMaterial(UICharacter character, int index)
        {
            StartCoroutine(_listClassMaterial[index].FilterClassMaterial(character));
            StartCoroutine(SelectDefaultButton(index));
        }

        private IEnumerator SelectDefaultButton(int index)
        {
            yield return new WaitUntil(() => _listClassMaterial[index].IsFilterClassMaterial);
            EnableButtonInteractable(true, index);
        }

        public void EnableButtonInteractable(bool isEnable, int index)
        {
            foreach (var button in _listClassMaterial[index].ListClassCharacter)
            {
                button.GetComponent<Button>().interactable = isEnable;
            }

            var firstItemGO = _listClassMaterial[index].ListClassCharacter[0].gameObject;
            EventSystem.current.SetSelectedGameObject(firstItemGO);
        }
    }
}
