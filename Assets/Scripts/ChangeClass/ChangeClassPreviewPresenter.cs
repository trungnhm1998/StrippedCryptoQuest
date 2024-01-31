using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.API;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Input;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassPreviewPresenter : MonoBehaviour
    {
        [SerializeField] private ChangeClassSyncData _syncData;
        [SerializeField] private PreviewCharacterAPI _previewCharacterAPI;
        [SerializeField] private ChangeNewClassAPI _changeNewClassAPI;
        [SerializeField] private ChangeNewClassBerserkerAPI _changeClassBerserkerAPI;
        [SerializeField] private ChangeClassPresenter _changeClassPresenter;
        [SerializeField] private CalculatorCharacterStats _calculatorFirstCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorSecondCharacterStats;
        [SerializeField] private CalculatorCharacterStats _calculatorTooltipCharacter;
        [SerializeField] private UIPreviewCharacter _previewNewClass;
        [SerializeField] private UIPreviewCharacter _previewNewClassStatus;
        [SerializeField] private UIChangeClassTooltip _preview;
        [SerializeField] private InitializeNewCharacter _initializeCharacter;
        public UnityAction<UICharacter> FirstClassMaterialEvent;
        public UnityAction<UICharacter> LastClassMaterialEvent;
        public UICharacter BaseUnitId1{ get; private set; }
        public UICharacter BaseUnitId2{ get; private set; }
        public UICharacter BerserkerMaterial { get; private set; }
        private TinyMessageSubscriptionToken _changeClassRequestSuccess;
        public List<int> _materialsId { get; private set; } = new();

        private void OnEnable()
        {
            FirstClassMaterialEvent += GetFirstClassMaterial;
            LastClassMaterialEvent += GetLastClassMaterial;
            _changeClassRequestSuccess = ActionDispatcher.Bind<ChangeNewClassDataRespond>(HandleChangeClassSuccess);
        }

        private void OnDisable()
        {
            FirstClassMaterialEvent -= GetFirstClassMaterial;
            LastClassMaterialEvent -= GetLastClassMaterial;
            ActionDispatcher.Unbind(_changeClassRequestSuccess);
        }

        private void HandleChangeClassSuccess(ChangeNewClassDataRespond obj)
        {
            _syncData.SetNewClassData(obj.ResponseData, _previewNewClassStatus);
            _initializeCharacter.GetStats(obj.ResponseData);
            GetDefaultExp(_previewNewClassStatus);
        }

        private void GetFirstClassMaterial(UICharacter character)
        {
            BaseUnitId1 = character;
        }

        private void GetLastClassMaterial(UICharacter character)
        {
            BaseUnitId2 = character;
        }

        public void FilterClassMaterial(UICharacter character, UIClassMaterial classMaterial)
        {
            StartCoroutine(classMaterial.FilterClassMaterial(character));
            StartCoroutine(SelectDefaultButton(classMaterial));
        }

        private IEnumerator SelectDefaultButton(UIClassMaterial classMaterial)
        {
            yield return new WaitUntil(() => classMaterial.IsFinishInstantiateData);
            var materialNumber = classMaterial.ListClassCharacter.Count;
            if (materialNumber != 0)
                EnableButtonInteractable(true, classMaterial);
        }

        public void SetFirstClassMaterial(UIClassMaterial classMaterial, UICharacter character)
        {
            FirstClassMaterialEvent.Invoke(character);
            EnableButtonInteractable(false, classMaterial);
            HideDetail();
            character.EnableButtonBackground(true);
        }

        public void SetLastClassMaterial(UIClassMaterial classMaterial, UICharacter character)
        {
            EnableButtonInteractable(false, classMaterial);
            LastClassMaterialEvent?.Invoke(character);
            character.EnableButtonBackground(true);
        }

        public void EnableButtonInteractable(bool isEnable, UIClassMaterial classMaterial)
        {
            foreach (var button in classMaterial.ListClassCharacter)
            {
                button.GetComponent<Button>().interactable = isEnable;
            }

            var firstItemGO = classMaterial.ListClassCharacter[0].gameObject;
            EventSystem.current.SetSelectedGameObject(firstItemGO);
        }

        public void PreviewData()
        {
            StartCoroutine(PreviewNewClassData());
            GetClassMaterialId();
        }

        public void SetBerserkerMaterial(UICharacter classMaterial)
        {
            BerserkerMaterial = classMaterial;
        }

        public void PreviewClassBerserkerData()
        {
            StartCoroutine(PreviewClassBerserker());
        }

        private void GetClassMaterialId()
        {
            _materialsId.Clear();
            _materialsId.Add(BaseUnitId1.Class.Id);
            _materialsId.Add(BaseUnitId2.Class.Id);
        }

        private IEnumerator PreviewNewClassData()
        {
            var avatar = _syncData.Avatar(BaseUnitId1, _changeClassPresenter.Occupation);
            StartCoroutine(_previewCharacterAPI.CoPreviewNewClass(BaseUnitId1, BaseUnitId2));
            CheckElementImage();
            yield return new WaitUntil(() => _previewCharacterAPI.IsFinishFetchData);
            _previewNewClass.PreviewCharacter(_previewCharacterAPI.Data, BaseUnitId1, avatar,
                CheckElementImage());
            _calculatorFirstCharacterStats.CalculatorStats(BaseUnitId1);
            _calculatorSecondCharacterStats.CalculatorStats(BaseUnitId2);
            GetDefaultExp(_previewNewClass);
        }

        private IEnumerator PreviewClassBerserker()
        {
            var avatar = _syncData.Avatar(BerserkerMaterial, _changeClassPresenter.Occupation);
            StartCoroutine(_previewCharacterAPI.CoPreviewClassBerserker(BerserkerMaterial));
            yield return new WaitUntil(() => _previewCharacterAPI.IsFinishFetchData);
            _previewNewClass.PreviewCharacter(_previewCharacterAPI.Data, BerserkerMaterial, avatar,
                true);
            _calculatorFirstCharacterStats.CalculatorStats(BerserkerMaterial);
            GetDefaultExp(_previewNewClass);
        }

        private void GetDefaultExp(UIPreviewCharacter character)
        {
            var requiredExp = _calculatorFirstCharacterStats.GetRequiredExp(0);
            var currentExp = _calculatorFirstCharacterStats.GetCurrentExp(0);
            character.UpdateExpBar(currentExp, requiredExp);
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
            return BaseUnitId1.Class.Elemental == BaseUnitId2.Class.Elemental;
        }
    }
}