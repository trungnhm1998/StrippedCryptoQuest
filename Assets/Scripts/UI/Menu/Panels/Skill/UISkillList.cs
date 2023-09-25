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
        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;

        private List<AbilityData> _skills = new();
        private int _skillCount = 0;
        private UISkillButton _defaultSelectedSkill;
        private float _verticalOffset;

        private void Awake()
        {
            _characterSelection.UpdateSkillListEvent += Init;
        }

        private void OnDestroy()
        {
            _characterSelection.UpdateSkillListEvent -= Init;
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

        private bool ShouldMoveUp => _scrollRect.content.anchoredPosition.y > _verticalOffset;

        private bool ShouldMoveDown =>
            _scrollRect.content.rect.height - _scrollRect.content.anchoredPosition.y
            > _scrollRect.viewport.rect.height + _verticalOffset / 2;

        private void DisplayNavigateArrows()
        {
            _upArrow.SetActive(ShouldMoveUp);
            _downArrow.SetActive(ShouldMoveDown);
        }

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
