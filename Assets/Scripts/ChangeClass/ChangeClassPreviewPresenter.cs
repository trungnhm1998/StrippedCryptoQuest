using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPreviewPresenter : MonoBehaviour
    {
        [SerializeField] private List<UIClassMaterial> _listClassMaterial;
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private PreviewCharacterAPI _previewCharacterAPI;
        [SerializeField] private ChangeNewClassAPI _changeNewClassAPI;
        [SerializeField] private ChangeClassPresenter _changeClassPresenter;
        [SerializeField] private UIPreviewClassMaterial _previewFirstClassMaterial;
        [SerializeField] private UIPreviewClassMaterial _previewLastClassMaterial;
        [SerializeField] private UIPreviewClassMaterial _showDetailClassMaterial;
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
            int materialNumber = _listClassMaterial[index].ListClassCharacter.Count;
            if (materialNumber != 0)
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
            var avatar = _syncData.Avatar(_firstClassMaterial, _changeClassPresenter.Occupation);
            _previewCharacterAPI.LoadDataToPreviewCharacter(_firstClassMaterial, _lastClassMaterial);
            CheckElementImage();
            yield return new WaitUntil(() => _previewCharacterAPI.IsFinishFetchData);
            _previewNewClass.PreviewCharacter(_previewCharacterAPI.Data, _firstClassMaterial, avatar, CheckElementImage());
            _previewFirstClassMaterial.PreviewCharacter(_firstClassMaterial);
            _previewLastClassMaterial.PreviewCharacter(_lastClassMaterial);
        }

        public void ChangeClass()
        {
            StartCoroutine(ChangeNewClassAPI());
        }

        private IEnumerator ChangeNewClassAPI()
        {
            _input.DisableInput();
            _changeNewClassAPI.ChangeNewClassData(_firstClassMaterial, _lastClassMaterial, _changeClassPresenter.Occupation);
            yield return new WaitUntil(() => _changeNewClassAPI.IsFinishFetchData);
            _input.EnableInput();

            if (_changeNewClassAPI.Data == null) yield break;
            _syncData.SetNewClassData(_changeNewClassAPI.Data, _previewNewClassStatus);
        }

        public void ShowDetail(UICharacter character)
        {
            if (!_showDetailClassMaterial.gameObject.activeSelf)
                _showDetailClassMaterial.gameObject.SetActive(true);
            _showDetailClassMaterial.PreviewCharacter(character);
        }

        public void HideDetail()
        {
            if (!_showDetailClassMaterial.gameObject.activeSelf) return;
            _showDetailClassMaterial.gameObject.SetActive(false);
        }

        private bool CheckElementImage()
        {
            return _firstClassMaterial.ElementImage == _lastClassMaterial.ElementImage;
        }
    }
}
