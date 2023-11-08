using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPreviewPresenter : MonoBehaviour
    {
        [SerializeField] private List<UIClassMaterial> _listClassMaterial;
        [SerializeField] private PreviewCharacterAPI _previewCharacterAPI;
        [SerializeField] private MockDataToChangeClassAPI _changeNewClassAPI;
        [SerializeField] private ChangeClassPresenter _changeClassPresenter;
        [SerializeField] private UIPreviewClassMaterial _previewFirstClassMaterial;
        [SerializeField] private UIPreviewClassMaterial _previewLastClassMaterial;
        [SerializeField] private UIPreviewCharacter _previewNewClass;
        [SerializeField] private UIPreviewCharacter _previewNewClassStatus;
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

        public void PreviewData()
        {
            StartCoroutine(PreviewNewClassData());
        }

        private IEnumerator PreviewNewClassData()
        {
            _previewCharacterAPI.LoadDataToPreviewCharacter(_firstClassMaterial, _lastClassMaterial);
            yield return new WaitUntil(() => _previewCharacterAPI.IsFinishFetchData);
            _previewNewClass.PreviewCharacter(_previewCharacterAPI.Data, _firstClassMaterial);
            _previewFirstClassMaterial.PreviewCharacter(_firstClassMaterial);
            _previewLastClassMaterial.PreviewCharacter(_lastClassMaterial);
        }

        public void ChangeClass()
        {
            StartCoroutine(ChangeNewClassAPI());
        }

        private IEnumerator ChangeNewClassAPI()
        {
            ActionDispatcher.Dispatch(new GetMockNftClassData { ForceRefresh = true });

            yield return new WaitForSeconds(1f);
            _previewNewClassStatus.PreviewNewCharacter(_changeNewClassAPI.Data, _firstClassMaterial);
        }
    }
}
