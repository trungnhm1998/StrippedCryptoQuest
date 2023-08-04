using CryptoQuest.Gameplay.Skill;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillAbility : MonoBehaviour, ICell
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _manaPointText;
        [field: SerializeField] public LocalizedString Description { get; private set; }
        private SkillInformation _skillInfo;

        public event UnityAction<SkillSO> Clicked;

        public void Init(SkillInformation skillInfo)
        {
            _skillInfo = skillInfo;
            _iconImage.sprite = skillInfo.SkillSO.Icon;
            _name.StringReference = skillInfo.SkillSO.Name;
            _manaPointText.text = skillInfo.SkillSO.Mana.ToString();
            Description = skillInfo.SkillSO.Description;
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_skillInfo.SkillSO);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
