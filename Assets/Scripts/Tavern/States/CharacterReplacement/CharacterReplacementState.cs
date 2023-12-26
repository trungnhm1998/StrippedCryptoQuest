using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Tavern.UI;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class CharacterReplacementState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<HeroSpec> _cachedGameData = new List<HeroSpec>();
        private List<HeroSpec> _cachedDboxData = new List<HeroSpec>();

        private static readonly int OverviewState = Animator.StringToHash("Overview");
        private static readonly int ConfirmState = Animator.StringToHash("Confirm Character Replacement");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.UICharacterReplacement.Contents.SetActive(true);
            _controller.UICharacterReplacement.SelectedGameItemsIds.Clear();
            _controller.UICharacterReplacement.SelectedDboxItemsIds.Clear();
            _controller.UICharacterReplacement.HandleListInteractable();
            UITavernItem.Pressed += _controller.UICharacterReplacement.Transfer;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetInGameHeroesSucceeded>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetInDboxHeroesSucceeded>(GetWalletCharacters);

            _controller.MerchantInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent += ViewCharacterDetails;

            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
        }

        private void GetInGameCharacters(GetInGameHeroesSucceeded obj)
        {
            _cachedGameData.Clear();
            foreach (var hero in obj.Heroes)
            {
                var newHero = ServiceProvider.GetService<IHeroResponseConverter>().Convert(hero);
                _cachedGameData.Add(newHero);
            }
            _controller.UIGameList.SetData(_cachedGameData);

            /*
             * TODO
             * check if any characters has the same id with the characters in party then enable lock tag
             */
        }

        private void GetWalletCharacters(GetInDboxHeroesSucceeded obj)
        {
            _cachedDboxData.Clear();
            foreach (var hero in obj.Heroes)
            {
                var newHero = ServiceProvider.GetService<IHeroResponseConverter>().Convert(hero);
                _cachedDboxData.Add(newHero);
            }
            _controller.UIWalletList.SetData(_cachedDboxData);
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

            _controller.UIGameList.SetData(_cachedGameData);
            _controller.UIWalletList.SetData(_cachedDboxData);
        }

        private void ViewCharacterDetails()
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UITavernItem>().InspectDetails();
        }

        protected override void OnExit()
        {
            _controller.UICharacterReplacement.StopHandleListInteractable();
            UITavernItem.Pressed -= _controller.UICharacterReplacement.Transfer;

            ActionDispatcher.Unbind(_getGameDataSucceedEvent);
            ActionDispatcher.Unbind(_getWalletDataSucceedEvent);

            _controller.MerchantInputManager.CancelEvent -= CancelCharacterReplacement;
            _controller.MerchantInputManager.NavigateEvent -= SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent -= SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent -= ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent -= ViewCharacterDetails;
        }
    }
}