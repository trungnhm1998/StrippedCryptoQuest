using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIStatusCharacter : MonoBehaviour
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private UIStatusMenu _statusMenu;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIStats _stats;
        [SerializeField] private Image _characterElement;
        [SerializeField] private UIEquipmentOverview _equipmentOverview;
        [SerializeField] private TMP_Text _level;
        private string _lvlTxtFormat = string.Empty;
        [SerializeField] private List<UIElementAttribute> _elementAttributes;

        private IParty _playerParty;

        private List<HeroDataSO> _activeMembersData = new();
        private List<AttributeSystemBehaviour> _activeMembersAttribute = new();

        private int _currentIndex = 0;

        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _activeMembersData.Count;
                _currentIndex = (value + count) % count;
            }
        }

        public void Init(IParty party)
        {
            _playerParty = party;
        }

        private void OnEnable()
        {
            Debug.Log("UIStatusCharacter OnEnable");
            ShowFirstCharacter();
        }

        private void ShowFirstCharacter()
        {
            var firstMember = _playerParty.Members[0];
            _characterElement.sprite = firstMember.Element.Icon;
            if (_lvlTxtFormat == string.Empty)
            {
                _lvlTxtFormat = _level.text;
            }

            _level.text = string.Format(_lvlTxtFormat, firstMember.Level);
            UpdateElementsStats(firstMember.CharacterComponent.AttributeSystem);
            _avatar.sprite = firstMember.Avatar;
        }

        private void UpdateElementsStats(AttributeSystemBehaviour attributeSystem)
        {
            for (int i = 0; i < _elementAttributes.Count; i++)
            {
                var elementUI = _elementAttributes[i];
                elementUI.SetStats(attributeSystem);
            }
        }

        // Code smell here, need to refactor later, violate DRY with UIHomeMenuSortCharacter
        private void LoadPartyMembers()
        {
            _activeMembersData.Clear();

            foreach (var member in _party.PlayerTeam.Members)
            {
                if (member.gameObject.activeSelf)
                {
                    member.TryGetComponent<ScriptableObjectStatsInitializer>(out var initializer);
                    var memberStats = initializer.DefaultStats as HeroDataSO;
                    _activeMembersData.Add(memberStats);
                    _activeMembersAttribute.Add(member.AttributeSystem);
                    _stats.SetAttributes(member.AttributeSystem);
                    _equipmentOverview.SetEquipment(_activeMembersData[CurrentIndex].Equipments);
                }
            }
        }

        public void ChangeCharacter(Vector2 direction)
        {
            if (direction.x > 0)
                CurrentIndex++;
            else if (direction.x < 0)
                CurrentIndex--;

            _avatar.sprite = _activeMembersData[CurrentIndex].Avatar;
            _stats.SetAttributes(_activeMembersAttribute[CurrentIndex]);
            _equipmentOverview.SetEquipment(_activeMembersData[CurrentIndex].Equipments);
        }
    }
}