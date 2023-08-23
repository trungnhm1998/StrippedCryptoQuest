using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.UI.Menu.Panels.Home;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
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
        [SerializeField] private UIEquipmentOverview _equipmentOverview;

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

        private void OnEnable()
        {
            _statusMenu.MenuOpenedEvent += LoadPartyMembers;
        }

        private void OnDisable()
        {
            _statusMenu.MenuOpenedEvent -= LoadPartyMembers;
        }

        // Code smell here, need to refactor later, violate DRY with UIHomeMenuSortCharacter
        private void LoadPartyMembers()
        {
            _activeMembersData.Clear();

            foreach (var member in _party.PlayerTeam.Members)
            {
                if (member.gameObject.activeSelf)
                {
                    member.TryGetComponent<StatsInitializer>(out var initializer);
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