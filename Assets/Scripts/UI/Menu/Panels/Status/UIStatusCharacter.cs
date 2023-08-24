using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public interface IStatsRenderer
    {
        public void SetParty(IPartyManager party);
    }

    public class UIStatusCharacter : MonoBehaviour, IStatsRenderer
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private UIStatusMenu _statusMenu;
        [SerializeField] private Image _avatar;
        [SerializeField] private UIStats _stats;
        [SerializeField] private SpriteRenderer _characterElement;
        [SerializeField] private UIEquipmentOverview _equipmentOverview;
        [SerializeField] private List<UIElementAttribute> _elementAttributes;

        private List<HeroDataSO> _activeMembersData = new();
        private List<AttributeSystemBehaviour> _activeMembersAttribute = new();
        private IPartyManager _playerParty;

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

        private void OnEnable()
        {
            Debug.Log("UIStatusCharacter OnEnable");
            ShowFirstCharacter();
        }

        private void ShowFirstCharacter()
        {
            // _playerParty.Slots[0].
        }

        private void OnDisable()
        {
        }

        public void SetParty(IPartyManager party)
        {
            _playerParty = party;
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