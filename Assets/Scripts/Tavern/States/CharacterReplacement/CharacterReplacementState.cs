using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Merchant;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Tavern.UI;
using IndiGames.Core.Common;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using HeroObject = CryptoQuest.Sagas.Objects.Character;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class CharacterReplacementState : StateMachineBehaviourBase
    {
        [SerializeField] private MerchantInput _merchantInput;
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Character Replacement");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UICharacterReplacement.Contents.SetActive(true);
            _controller.UICharacterReplacement.SelectedGameItemsIds.Clear();
            _controller.UICharacterReplacement.SelectedDboxItemsIds.Clear();
            UITavernItem.Pressed += _controller.UICharacterReplacement.Transfer;

            _merchantInput.CancelEvent += CancelCharacterReplacement;
            _merchantInput.NavigateEvent += SwitchToOtherListRequested;
            _merchantInput.ExecuteEvent += SendItemsRequested;
            _merchantInput.ResetEvent += ResetTransferRequested;
            _merchantInput.InteractEvent += ViewCharacterDetails;
        }

        private void ConvertAndPassDataToUi(HeroObject[] heroes, List<HeroSpec> cacheList, UICharacterList listUI)
        {
            cacheList.Clear();
            foreach (var hero in heroes)
            {
                var newHero = ServiceProvider.GetService<IHeroResponseConverter>().Convert(hero);
                cacheList.Add(newHero);
            }

            listUI.SetData(cacheList);
            _controller.UICharacterReplacement.HandleListInteractable();
        }

        private void CancelCharacterReplacement()
        {
            _controller.UICharacterReplacement.Contents.SetActive(false);
            StateMachine.Play(OverviewState);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UICharacterReplacement.SwitchList(direction);
        }

        private void SendItemsRequested()
        {
            if (_controller.UICharacterReplacement.SelectedGameItemsIds.Count == 0 &&
                _controller.UICharacterReplacement.SelectedDboxItemsIds.Count == 0) return;
            StateMachine.Play(ConfirmState);
        }

        private void ResetTransferRequested()
        {
            if (_controller.UICharacterReplacement.SelectedGameItemsIds.Count == 0 &&
                _controller.UICharacterReplacement.SelectedDboxItemsIds.Count == 0) return;

            _controller.UICharacterReplacement.SelectedGameItemsIds.Clear();
            _controller.UICharacterReplacement.SelectedDboxItemsIds.Clear();

            _controller.UIGameList.UpdateList();
            _controller.UIDboxList.UpdateList();
            _controller.UIGameList.HandleInPartyHeroes();
        }

        private void ViewCharacterDetails()
        {
            var currentItem = EventSystem.current.currentSelectedGameObject.GetComponent<UITavernItem>();
            currentItem.OnInspectDetails();
            _controller.SpecInitializer.Init(currentItem.Hero);
        }

        protected override void OnExit()
        {
            _controller.UICharacterReplacement.StopHandleListInteractable();
            UITavernItem.Pressed -= _controller.UICharacterReplacement.Transfer;

            _merchantInput.CancelEvent -= CancelCharacterReplacement;
            _merchantInput.NavigateEvent -= SwitchToOtherListRequested;
            _merchantInput.ExecuteEvent -= SendItemsRequested;
            _merchantInput.ResetEvent -= ResetTransferRequested;
            _merchantInput.InteractEvent -= ViewCharacterDetails;
        }
    }
}