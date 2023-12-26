using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Character;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class TransferCharacter : MonoBehaviour
    {
        [SerializeField] private PartySO _partySO;
        [SerializeField] private HeroInventorySO _heroesInventorySO;
        private IPartyController _partyController;
        private HeroSpec _heroSpec;
        private int _memberId;

        public void InitializeParty(List<int> heroesId, HeroSpec spec)
        {
            bool isPartyMember = false;
            _heroSpec = spec;
            _partyController = ServiceProvider.GetService<IPartyController>();

            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                var member = _partyController.Slots[i];

                foreach (var id in heroesId)
                {
                    if (TryRemoveHeroFromParty(member, id))
                    {
                        isPartyMember = true;
                        _memberId = i;
                        break;
                    }
                }
            }
            if (isPartyMember) SetCharacterToParty(_memberId);
            else SetCharacterToInventory();

            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
        }

        private bool TryRemoveHeroFromParty(PartySlot member, int heroId)
        {
            if (!member.IsValid()) return false;
            if (member.Spec.Hero.Id == heroId)
            {
                member.Spec.Hero = new();
                RemoveEquipmentsFromHeroAndAddBackToInventory(member.Spec);
                return true;
            }
            return false;
        }

        private void RemoveEquipmentsFromHeroAndAddBackToInventory(PartySlotSpec partySlotSpec)
        {
            var equippingItems = FilterUniqueEquippingItems(partySlotSpec);
            foreach (var item in equippingItems) ActionDispatcher.Dispatch(new AddEquipmentAction(item));
            partySlotSpec.EquippingItems.Slots = new();
        }

        private static HashSet<IEquipment> FilterUniqueEquippingItems(PartySlotSpec partySlotSpec)
        {
            var equippingItems = new HashSet<IEquipment>();
            foreach (var equipmentSlot in partySlotSpec.EquippingItems.Slots)
            {
                if (equipmentSlot.IsValid() == false) continue;
                var item = equipmentSlot.Equipment;
                equippingItems.Add(item);
            }
            return equippingItems;
        }

        private void SetCharacterToInventory()
        {
            _heroesInventorySO.OwnedHeroes.Add(_heroSpec);
        }

        private void SetCharacterToParty(int index)
        {
            var newCharacter = _partyController.Slots[index];
            newCharacter.Spec.Hero = _heroSpec;
            newCharacter.Init(newCharacter.Spec);
            UpdatePartySO();
        }

        private void UpdatePartySO()
        {
            var party = GetHeroesInParty();
            _partySO.SetParty(party.ToArray());
        }

        private List<PartySlotSpec> GetHeroesInParty()
        {
            return (from slot in _partySO.GetParty()
                    where slot.IsValid()
                    select new PartySlotSpec() { Hero = slot.Hero, }).ToList();
        }
    }
}
