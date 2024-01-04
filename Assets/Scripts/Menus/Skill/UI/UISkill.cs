using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Gameplay;
using CryptoQuest.Menu;
using IndiGames.Core.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UISkill : MonoBehaviour
    {
        public static event Action<UISkill> InspectingSkillEvent;

        [SerializeField] private Image _skillIcon;
        [SerializeField] private MultiInputButton _skillButton;
        [SerializeField] private LocalizeStringEvent _skillName;
        [SerializeField] private TMP_Text _skillNameText;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Color _disableColor;

        private Color _normalColor;
        private IScenarioChecker _scenarioChecker;
        public CastSkillAbility Skill { get; private set; }

        public bool Interactable
        {
            set => _skillButton.interactable = value;
        }

        private void Awake()
        {
            _normalColor = _skillNameText.color;
        }

        private void OnEnable()
        {
            _skillButton.Selected += OnSelected;
            _scenarioChecker = ServiceProvider.GetService<IScenarioChecker>();
        }

        private void OnDisable()
        {
            _skillButton.Selected -= OnSelected;
        }

        public void Init(CastSkillAbility skill)
        {
            Skill = skill;
            _skillName.StringReference = skill.SkillName;
            _cost.text = skill.SkillInfo.Cost.ToString();
            bool isAllowed = _scenarioChecker.IsCorrectScenario(skill.SkillInfo.UsageScenarioSO);
            SetDisable(!isAllowed);
        }

        private void OnSelected()
        {
            InspectingSkillEvent?.Invoke(this);
        }

        private void SetDisable(bool value)
        {
            _skillNameText.color = value ? _disableColor : _normalColor;
            _cost.color = value ? _disableColor : _normalColor;
        }
    }
}