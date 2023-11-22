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
        [SerializeField] private CalculatorCharacterStats _calculatorFirstCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorSecondCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorTooltipCharacter;
        [SerializeField] private UIPreviewCharacter _previewNewClass;
        [SerializeField] private UIPreviewCharacter _previewNewClassStatus;
        [SerializeField] private UIChangeClassTooltip _preview;
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
            var materialNumber = _listClassMaterial[index].ListClassCharacter.Count;
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
            _calculatorFirstCharacterStats.CalculatorStats(_firstClassMaterial);
            _calculatorSecondCharacterStats.CalculatorStats(_lastClassMaterial);
            GetDefaultExp(_previewNewClass);
        }

        private void GetDefaultExp(UIPreviewCharacter character)
        {
            var requiredExp = _calculatorFirstCharacterStats.GetRequiredExp(0);
            var currentExp = _calculatorFirstCharacterStats.GetCurrentExp(0);
            character.UpdateExpBar(currentExp, requiredExp);
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
            GetDefaultExp(_previewNewClassStatus);
        }

        public void ShowDetail(UICharacter character)
        {
            if (!_calculatorTooltipCharacter.gameObject.activeSelf)
            {
                _calculatorTooltipCharacter.gameObject.SetActive(true);
                _preview.ShowTooltip(character);
            }
            _calculatorTooltipCharacter.CalculatorStats(character);
        }

        public void HideDetail()
        {
            if (!_calculatorTooltipCharacter.gameObject.activeSelf) return;
            _calculatorTooltipCharacter.gameObject.SetActive(false);
        }

        private bool CheckElementImage()
        {
            return _firstClassMaterial.Class.Elemental == _lastClassMaterial.Class.Elemental;
        }
    }
}
