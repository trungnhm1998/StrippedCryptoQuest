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
        public Action<int> GotLevelEvent;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _equipmentPrefab;
        [SerializeField] private GameObject _previewObject;
        [SerializeField] private GameObject _confirmPanel;
        [SerializeField] private RectTransform _upgradeEquipmentPanel;
        [SerializeField] private Transform _selectedEquipmentPanel;
        [SerializeField] private Transform _defaultPanel;
        [SerializeField] private Transform _resultPanel;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _inValidColor;
        private int _selectLevelToUpgrade;
        private int _currentLevel;
        private int _levelToUpgrade;
        private bool _isValid;
        private float _gold;

        public void SetValue(int level, IUpgradeEquipment item)
        {
            _currentLevel += level;
            if (_currentLevel > item.Level && _currentLevel <= item.Equipment.Def.MaxLevel)
                _selectLevelToUpgrade = _currentLevel;

            else
                _currentLevel = _selectLevelToUpgrade;
            _level.text = _selectLevelToUpgrade.ToString();
            CostToUpgrade(item);
        }

        private void CostToUpgrade(IUpgradeEquipment item)
        {
            _levelToUpgrade = _currentLevel - item.Level;
            _isValid = _gold >= _levelToUpgrade * item.Cost;
            _cost.color = _isValid ? _validColor : _inValidColor;
            _cost.text = $"{_levelToUpgrade * item.Cost} G";
            GotLevelEvent?.Invoke(_levelToUpgrade);
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

        public void SetLevel(IUpgradeEquipment item, UIEquipmentDetails details)
        {
            item.Equipment.SetLevel(_currentLevel);
            details.RenderData(item.Equipment);
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

        public void SelectedEquipment(UIUpgradeItem item, float gold)
        {
            _gold = gold;
            item.transform.SetParent(_upgradeEquipmentPanel);
            _selectLevelToUpgrade = item.UpgradeEquipment.Level + 1;
            _currentLevel = _selectLevelToUpgrade;
            _level.text = _currentLevel.ToString();
            CostToUpgrade(item.UpgradeEquipment);
        }

        private void ChangeLocation(Transform transform)
        {
            _previewObject.transform.SetParent(transform);
            _previewObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        public void ShowConfirmPanel(bool isShowPanel) => _confirmPanel.SetActive(isShowPanel);
    }
}