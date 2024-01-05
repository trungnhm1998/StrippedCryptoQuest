using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Character;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Hero.ChangeClass
{
    public class ChangeHeroClassManager : MonoBehaviour
    {
        [SerializeField] private HeroParameter _defaultHeroClassParameter;
        [SerializeField] private HeroParameter _newHeroClassParameter;
        [SerializeField] private VoidEventChannelSO _changeClassEvent;
        private IPartyController _partyController;
        private HeroSpec _heroSpec;

        private void OnEnable()
        {
            _changeClassEvent.EventRaised += ChangeClassHero;
        }

        private void OnDisable()
        {
            _changeClassEvent.EventRaised -= ChangeClassHero;
        }

        private void ChangeClassHero()
        {
            _partyController = ServiceProvider.GetService<IPartyController>();

            foreach (var member in _partyController.Slots)
            {
                if (!member.IsValid()) continue;

                if (member.Spec.Hero.Class == _defaultHeroClassParameter.Class)
                {
                    RemoveEquipmentsAndAddBackToInventory(member.Spec);
                    UpdateHeroStatsAndClass(member.Spec.Hero);
                    ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
                    return;
                }
            }
        }

        private void RemoveEquipmentsAndAddBackToInventory(PartySlotSpec partySlotSpec)
        {
            var equippingItems = FilterUniqueEquippingItems(partySlotSpec);
            foreach (var item in equippingItems)
            {
                ActionDispatcher.Dispatch(new AddEquipmentAction(item));
            }

            partySlotSpec.EquippingItems.Slots.Clear();
        }

        private void UpdateHeroStatsAndClass(HeroSpec hero)
        {
            hero.Stats = _newHeroClassParameter.Stats;
            hero.Class = _newHeroClassParameter.Class;
            hero.Experience = 0;
        }

        private static HashSet<IEquipment> FilterUniqueEquippingItems(PartySlotSpec partySlotSpec)
        {
            var equippingItems = new HashSet<IEquipment>();
            foreach (var equipmentSlot in partySlotSpec.EquippingItems.Slots)
            {
                if (equipmentSlot.IsValid())
                {
                    equippingItems.Add(equipmentSlot.Equipment);
                }
            }

            return equippingItems;
        }
    }
}