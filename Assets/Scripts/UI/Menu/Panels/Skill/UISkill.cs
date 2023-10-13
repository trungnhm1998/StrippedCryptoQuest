using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using CryptoQuest.Character.Ability;
using System;
using TMPro;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkill : MonoBehaviour
    {
        public static event Action<CastSkillAbility> ConfirmSkillEvent;
        public static event Action<CastSkillAbility> InspectingSkillEvent;

        [SerializeField] private Image _skillIcon;
        [SerializeField] private UISkillButton _skillButton;
        [SerializeField] private LocalizeStringEvent _skillName;
        [SerializeField] private TMP_Text _skillNameText;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Color _disableColor;

        private Color _normalColor;
        private bool _isDisabled = false;

        public CastSkillAbility CachedSkill { get; private set; }

        private void Awake()
        {
            _normalColor = _skillNameText.color;
        }

        private void OnEnable()
        {
            _skillButton.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _skillButton.Selected -= OnSelected;
        }

        public void Init(CastSkillAbility skill)
        {
            CachedSkill = skill;
            _skillName.StringReference = skill.Parameters.SkillName;
            _cost.text = skill.Parameters.Cost.ToString();

            SetDisable(!skill.Parameters.UsageScenarioSO.HasFlag(EAbilityUsageScenario.Field));
            
            _skillButton.onClick.RemoveAllListeners();
            _skillButton.onClick.AddListener(OnPressButton);
        }

        private void OnPressButton()
        {
            CachedSkill.TargetType.RaiseEvent(CachedSkill);
            ConfirmSkillEvent?.Invoke(CachedSkill);
        }

        private void OnSelected()
        {
            InspectingSkillEvent?.Invoke(CachedSkill);
        }

        private void SetDisable(bool value)
        {
            _isDisabled = value;
            _skillButton.interactable = value;
            _skillNameText.color = value ? _disableColor : _normalColor;
            _cost.color = value ? _disableColor : _normalColor;
        }
    }
}
