using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.System;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillList : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        public static UnityAction EnterSkillSelectionEvent;

        [SerializeField] private UICharacterSelection _characterSelection;

        [Header("Scroll View Configs")]
        [SerializeField] private RecyclableScrollRect _scrollRect;
        [SerializeField] private RectTransform _singleItemRect;
        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;

        private List<AbilityData> _skills = new();
        private int _skillCount = 0;
        private UISkillButton _defaultSelectedSkill;

        private RectTransform _skillListViewport;
        private float _verticalOffset;
        private float _lowerBound;
        private float _upperBound;

        private void Awake()
        {
            _characterSelection.UpdateSkillListEvent += Init;
            UISkillButton.InspectingRow += AutoScroll;

            SetupScrollViewInfo();
        }

        private void OnDestroy()
        {
            _characterSelection.UpdateSkillListEvent -= Init;
            UISkillButton.InspectingRow -= AutoScroll;
        }

        private void OnEnable()
        {
            CleanUpScrollView();
            Init(ServiceProvider.GetService<IPartyController>().Party.Members[0]);
        }

        private void Init(CharacterSpec characterSpec, bool isAnotherChar = false)
        {
            GetSkills(characterSpec.GetAvailableSkills());

            if (isAnotherChar)
            {
                StartCoroutine(RefreshSkillList());
                return;
            }

            _scrollRect.Initialize(this);
        }

        private void GetSkills(List<AbilityData> skills)
        {
            _skills = skills;
            _skillCount = skills.Count;
        }

        private IEnumerator RefreshSkillList()
        {
            CleanUpScrollView();
            yield return null;

            _scrollRect.Initialize(this);
            _scrollRect.ReloadData();

            yield return new WaitForSeconds(.1f);
            OnSelectFirstSkill();
        }

        private void CleanUpScrollView()
        {
            if (_scrollRect.content.transform.childCount > 0)
                foreach (Transform child in _scrollRect.content.transform)
                {
                    Destroy(child.gameObject);
                }
        }

        private void OnSelectFirstSkill()
        {
            EnterSkillSelectionEvent?.Invoke();

            _defaultSelectedSkill = _scrollRect.content.GetChild(0).GetComponent<UISkillButton>();
            _defaultSelectedSkill.Select();
        }

        #region Auto Scroll View Setup
        private void SetupScrollViewInfo()
        {
            _verticalOffset = _singleItemRect.rect.height;
            _skillListViewport = _scrollRect.viewport;
            var position = _skillListViewport.position;
            var rect = _skillListViewport.rect;
            _lowerBound = position.y - rect.height / 2;
            _upperBound = position.y + rect.height / 2 + _verticalOffset;
        }

        private void AutoScroll(Button button)
        {
            var selectedRowPositionY = button.transform.position.y;

            if (selectedRowPositionY <= _lowerBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.up * _verticalOffset;
            }
            else if (selectedRowPositionY >= _upperBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.down * _verticalOffset;
            }

            DisplayNavigateArrows();
        }

        private bool ShouldMoveUp => _scrollRect.content.anchoredPosition.y > _verticalOffset;

        private bool ShouldMoveDown =>
            _scrollRect.content.rect.height - _scrollRect.content.anchoredPosition.y
            > _scrollRect.viewport.rect.height + _verticalOffset / 2;

        private void DisplayNavigateArrows()
        {
            _upArrow.SetActive(ShouldMoveUp);
            _downArrow.SetActive(ShouldMoveDown);
        }
        #endregion

        #region Recyclable Scroll View Setup
        public int GetItemCount()
        {
            return _skillCount;
        }

        public void SetCell(ICell cell, int index)
        {
            var skill = cell as UISkill;
            skill.Configure(_skills[index]);
        }
        #endregion

        #region SkillSelectionState Setup
        public void Init()
        {
            OnSelectFirstSkill();
        }
        #endregion
    }
}
