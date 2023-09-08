using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterButton : MultiInputButton
    {
        public static UnityAction<GameObject> SelectCharacterEvent;

        [SerializeField] private GameObject _selectingEffect;

        private GameObject _selectedObject;

        protected override void Awake()
        {
            base.Awake();
            UICharacterSelection.EnterCharacterSelectionEvent += EnableButton;
            UISkillList.EnterSkillSelectionEvent += DisableButton;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UICharacterSelection.EnterCharacterSelectionEvent -= EnableButton;
            UISkillList.EnterSkillSelectionEvent -= DisableButton;
        }

        private void EnableButton()
        {
            interactable = true;
        }

        private void DisableButton()
        {
            interactable = false;
        }

        public void CharacterSelected()
        {
            SelectCharacterEvent?.Invoke(_selectedObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _selectedObject = eventData.selectedObject;
            _selectingEffect.SetActive(false);
        }
    }
}
