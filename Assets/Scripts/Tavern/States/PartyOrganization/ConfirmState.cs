using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class ConfirmState : StateMachineBehaviourBase
    {
        [SerializeField] private HeroInventorySO _heroInventorySO;
        [SerializeField] private PartySO _partySO;
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _moreThan3Msg;

        private TavernController _controller;

        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");
        private IInventoryController _inventoryController;
        private const int MAX_NFT_HEROES_IN_PARTY = 3;

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
            _controller.MerchantInputManager.CancelEvent -= CancelTransmission;
            _controller.MerchantInputManager.SubmitEvent -= TurnOffDialogueIfThereAreMoreThan3Heroes;

            if (_controller.DialogsManager.ChoiceDialog == null) return;
            _controller.DialogsManager.ChoiceDialog.Hide();
        }

        private void CancelTransmission()
        {
            StateMachine.Play(PartyOrganizationState);
        }

        private void YesButtonPressed()
        {
            if (ValidateCurrentNumberOfHeroesAddingToParty()) return;
            ProceedToSendCharacters();
            _controller.UIPartyOrganization.ConfirmedTransmission();
            StateMachine.Play(PartyOrganizationState);
        }

        private bool ValidateCurrentNumberOfHeroesAddingToParty()
        {
            var selectedNonPartyCharacters = _controller.UIPartyOrganization.SelectedNonPartyCharacterIds.Count;
            var selectedInPartyCharacters = _controller.UIPartyOrganization.SelectedPartyCharacterIds.Count;

            var count = 0;
            foreach (var partySlot in _partySO.GetParty())
            {
                var isMain = partySlot.Hero.Id == 0;
                if (isMain) continue;
                if (partySlot.IsValid()) count++;
            }

            var numberOfHeroesInParty = count + selectedNonPartyCharacters - selectedInPartyCharacters;
            if (numberOfHeroesInParty <= MAX_NFT_HEROES_IN_PARTY) return false;

            _controller.MerchantInputManager.SubmitEvent += TurnOffDialogueIfThereAreMoreThan3Heroes;
            _controller.DialogsManager.ChoiceDialog.Hide();
            _controller.DialogsManager.Dialogue
                .SetMessage(_moreThan3Msg)
                .Show();

            return true;
        }

        private void TurnOffDialogueIfThereAreMoreThan3Heroes()
        {
            _controller.DialogsManager.Dialogue.Hide();
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

            switch (selectedNonPartyCharacters.Count)
            {
                case > 0 when selectedInPartyCharacters.Count > 0:
                    RemoveHeroesFromParty(selectedInPartyCharacters);
                    AddHeroesToParty(selectedNonPartyCharacters);
                    break;
                case > 0:
                    AddHeroesToParty(selectedNonPartyCharacters);
                    break;
                case <= 0 when selectedInPartyCharacters.Count > 0:
                    RemoveHeroesFromParty(selectedInPartyCharacters);
                    break;
            }
        }

        private void AddHeroesToParty(List<int> selectedHeroesToTransfer)
        {
            var finalParty = GetHeroesInParty();
            for (var index = finalParty.Count - 1; index >= 0; index--)
            {
                if (!finalParty[index].Hero.IsValid()) finalParty.RemoveAt(index);
            }

            var partySlotSpecs = new List<PartySlotSpec>();

            foreach (var selectedHeroId in selectedHeroesToTransfer)
                FindSelectedHeroesThenCacheInAnotherListAndRemoveFromInventory(selectedHeroId, partySlotSpecs);

            finalParty.AddRange(partySlotSpecs);
            _partySO.SetParty(finalParty.ToArray());
            ReinitializeParty();
        }

        private void FindSelectedHeroesThenCacheInAnotherListAndRemoveFromInventory(int selectedHeroId,
            List<PartySlotSpec> partySlotSpecs)
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
            }
        }

        private void RemoveHeroesFromParty(List<int> selectedHeroesToTransfer)
        {
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
            }
        }

        private void RemoveHeroFromPartyAndAddToHeroesInventory(List<PartySlotSpec> partySlotSpecs, HeroSpec hero,
            int index)
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