using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Character;
using CryptoQuest.UI.Menu.Panels.Item;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UICharacterButton : MultiInputButton
    {
        public static UnityAction<GameObject> SelectCharacterEvent;
        public static UnityAction<HeroBehaviour> InspectingCharacterEvent;

        [SerializeField] private GameObject _selectingEffect;

        private GameObject _selectedObject;
        private UICharacterInfoPanel _characterInfo;

        protected override void Awake()
        {
            base.Awake();
            UICharacterSelection.EnterCharacterSelectionEvent += EnableButton;
            UISkillList.EnterSkillSelectionEvent += DisableButton;
            _characterInfo = GetComponent<UICharacterInfoPanel>();
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
            if (_characterInfo != null && _characterInfo.Hero.IsValid())
                InspectingCharacterEvent?.Invoke(_characterInfo.Hero);
        }
    }
}
