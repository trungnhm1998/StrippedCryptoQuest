using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeEquipment : MonoBehaviour
    {
        public Action<UIUpgradeItem> OnSelected;
        public Action<UIUpgradeItem> OnSubmit;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _equipmentPrefab;
        [SerializeField] private GameObject _previewObject;
        [SerializeField] private RectTransform _upgradeEquipmentPanel;
        [SerializeField] private Transform _selectedEquipmentPanel;
        [SerializeField] private Transform _defaultPanel;
        [SerializeField] private Transform _resultPanel;
        [SerializeField] private TextMeshProUGUI _levelToUpgrade;
        private int _selectLevelToUpgrade;
        private int _currentLevel;

        public void SetValue(int level, IUpgradeEquipment item)
        {
            _currentLevel += level;
            if (_currentLevel >= item.Level && _currentLevel <= item.Equipment.Def.MaxLevel)
                _selectLevelToUpgrade = _currentLevel;

            else
                _currentLevel = _selectLevelToUpgrade;
            _levelToUpgrade.text = _selectLevelToUpgrade.ToString();
        }

        private void OnItemPressed(UIUpgradeItem item)
        {
            OnSubmit?.Invoke(item);
        }
        private void OnItemSelected(UIUpgradeItem item)
        {
            OnSelected?.Invoke(item);
        }

        public void InstantiateData(IUpgradeModel model)
        {
            ChangeLocation(_defaultPanel);
            CleanUpScrollView();
            for (int i = 0; i < model.ListEquipment.Count; i++)
            {
                var obj = Instantiate(_equipmentPrefab, _scrollRect.content).GetComponent<UIUpgradeItem>();
                obj.ConfigureCell(model.ListEquipment[i]);
                obj.OnItemSelected += OnItemSelected;
                obj.OnSubmit += OnItemPressed;
            }
            StartCoroutine(SelectDefaultButton());
        }

        public void SetLevel(IUpgradeEquipment item)
        {
            item.Equipment.SetLevel(_selectLevelToUpgrade);
            ChangeLocation(_resultPanel);
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in _selectedEquipmentPanel)
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            if (_scrollRect.content.childCount == 0) yield break;
            var firstItemGO = _scrollRect.content.GetChild(0).gameObject.GetComponent<UIUpgradeItem>();
            EventSystem.current.SetSelectedGameObject(firstItemGO.gameObject);
            OnSelected?.Invoke(firstItemGO);
        }

        public void SelectedEquipment(UIUpgradeItem item)
        {
            item.transform.SetParent(_upgradeEquipmentPanel);
            _currentLevel = item.UpgradeEquipment.Level;
            _levelToUpgrade.text = _currentLevel.ToString();
        }

        private void ChangeLocation(Transform transform)
        {
            _previewObject.transform.SetParent(transform);
            _previewObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}