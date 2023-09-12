using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillButton : MultiInputButton
    {
        public static event UnityAction<Button> InspectingRow;
        public static event UnityAction<AbilityData> SelectingSkillEvent;

        [SerializeField] private UISkill _singleSkill;

        protected override void Awake()
        {
            base.Awake();
            UISkillList.EnterSkillSelectionEvent += EnableButton;
            UICharacterSelection.EnterCharacterSelectionEvent += DisableButton;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UISkillList.EnterSkillSelectionEvent -= EnableButton;
            UICharacterSelection.EnterCharacterSelectionEvent -= DisableButton;
        }

        private void EnableButton()
        {
            interactable = true;
        }

        private void DisableButton()
        {
            interactable = false;
        }

        /// <summary>
        /// Activate an ability in Skill Menu.
        /// </summary>
        public void UseAbility()
        {
            Debug.Log($"Skill selected");
        }

        public override void OnSelect(BaseEventData eventData)
        {
            SelectingSkillEvent?.Invoke(_singleSkill.CachedAbilityData);
            InspectingRow?.Invoke(this);
            base.OnSelect(eventData);
        }
    }
}
