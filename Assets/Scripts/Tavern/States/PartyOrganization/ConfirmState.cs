using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class ConfirmState : StateMachineBehaviourBase
    {
        [SerializeField] private HeroInventorySO _heroInventorySO;
        [SerializeField] private PartySO _partySO;
        [SerializeField] private LocalizedString _confirmMessage;

        private TavernController _controller;

        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");
        private IInventoryController _inventoryController;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();

            _controller.MerchantInputManager.CancelEvent += CancelTransmission;

            _controller.UIParty.SetInteractableAllButtons(false);
            _controller.UINonParty.SetInteractableAllButtons(false);

            _controller.DialogsManager.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_confirmMessage)
                .Show();
        }

        protected override void OnExit()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
        }

        private void CancelTransmission()
        {
            StateMachine.Play(PartyOrganizationState);
        }

        private void YesButtonPressed()
        {
            ProceedToSendCharacters();
            _controller.UIPartyOrganization.ConfirmedTransmission();
            StateMachine.Play(PartyOrganizationState);
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            StateMachine.Play(PartyOrganizationState);
        }

        private void ProceedToSendCharacters()
        {
            List<int> selectedNonPartyCharacters = _controller.UIPartyOrganization.SelectedNonPartyCharacterIds;
            List<int> selectedInPartyCharacters = _controller.UIPartyOrganization.SelectedPartyCharacterIds;

            ActionDispatcher.Dispatch(new ShowLoading());

            switch (selectedNonPartyCharacters.Count)
            {
                case > 0 when selectedInPartyCharacters.Count > 0:
                    Debug.Log($"@@@@@@@@@@@@@ case 1");
                    break;
                case > 0:
                    AddHeroesToParty(selectedNonPartyCharacters);
                    Debug.Log($"@@@@@@@@@@@@@ case 2");
                    break;
                case <= 0 when selectedInPartyCharacters.Count > 0:
                    RemoveHeroesFromParty(selectedInPartyCharacters);
                    Debug.Log($"@@@@@@@@@@@@@ case 3");
                    break;
            }
        }

        private void AddHeroesToParty(List<int> selectedHeroesToTransfer)
        {
            var finalParty = GetHeroesInParty();
            var partySlotSpecs = new List<PartySlotSpec>();

            foreach (var selectedHeroId in selectedHeroesToTransfer)
            {
                for (var index = _heroInventorySO.OwnedHeroes.Count - 1; index >= 0; index--)
                {
                    var hero = _heroInventorySO.OwnedHeroes[index];
                    if (selectedHeroId != hero.Id) continue;
                    partySlotSpecs.Add(new PartySlotSpec()
                    {
                        Hero = hero
                    });
                    _heroInventorySO.OwnedHeroes.Remove(hero);
                    Debug.Log($"############## partySlotSpecs={partySlotSpecs.Count}, removedHero={hero.Id}");
                }
            }

            finalParty.AddRange(partySlotSpecs);
            _partySO.SetParty(finalParty.ToArray());
            ReinitializeParty();
        }

        private void RemoveHeroesFromParty(List<int> selectedHeroesToTransfer)
        {
            var finalParty = GetHeroesInParty();
            var partySlotSpecs = new List<PartySlotSpec>();

            foreach (var selectedHeroId in selectedHeroesToTransfer)
                FindAndRemoveHeroFromParty(selectedHeroId, partySlotSpecs);
            ReinitializeParty();
        }

        private void FindAndRemoveHeroFromParty(int selectedHeroId, List<PartySlotSpec> partySlotSpecs)
        {
            for (var index = _partySO.GetParty().Length - 1; index >= 0; index--)
            {
                var partySlotSpec = _partySO.GetParty()[index];
                var hero = partySlotSpec.Hero;
                if (selectedHeroId != hero.Id) continue;
                RemoveHeroFromPartyAndAddToHeroesInventory(partySlotSpecs, hero, index);
                RemoveEquipmentsFromHeroAndAddBackToInventory(partySlotSpec);

                Debug.Log($"############## partySlotSpecs={partySlotSpecs.Count}, removedHero={hero.Id}");
            }
        }

        private void RemoveHeroFromPartyAndAddToHeroesInventory(List<PartySlotSpec> partySlotSpecs, HeroSpec hero, int index)
        {
            partySlotSpecs.Add(new PartySlotSpec()
            {
                Hero = hero
            });
            _heroInventorySO.OwnedHeroes.Add(hero);
            _partySO.GetParty()[index].Hero = new();
        }

        private void RemoveEquipmentsFromHeroAndAddBackToInventory(PartySlotSpec partySlotSpec)
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            var equippingItems = FilterUniqueEquippingItems(partySlotSpec);
            foreach (var item in equippingItems) item.AddToInventory(_inventoryController);
            partySlotSpec.EquippingItems.Slots = new();
        }

        private static HashSet<EquipmentInfo> FilterUniqueEquippingItems(PartySlotSpec partySlotSpec)
        {
            var equippingItems = new HashSet<EquipmentInfo>();
            foreach (var equipmentSlot in partySlotSpec.EquippingItems.Slots)
            {
                if (equipmentSlot.IsValid() == false) continue;
                var item = equipmentSlot.Equipment;
                equippingItems.Add(item);
            }

            return equippingItems;
        }

        private static void ReinitializeParty()
        {
            var partyController = ServiceProvider.GetService<IPartyController>();
            partyController?.Init();
        }

        private List<PartySlotSpec> GetHeroesInParty()
        {
            return (from slot in _partySO.GetParty()
                where slot.IsValid()
                select new PartySlotSpec() { Hero = slot.Hero, }).ToList();
        }
    }
}