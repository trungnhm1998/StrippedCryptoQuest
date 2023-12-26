using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Sagas.Character;
using CryptoQuest.Tavern.UI;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using HeroObject = CryptoQuest.Sagas.Objects.Character;

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
            UITavernItem.Pressed += _controller.UICharacterReplacement.Transfer;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetInGameHeroesSucceeded>(GetInGameCharacters);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetInDboxHeroesSucceeded>(GetInDboxCharacters);

            _controller.MerchantInputManager.CancelEvent += CancelCharacterReplacement;
            _controller.MerchantInputManager.NavigateEvent += SwitchToOtherListRequested;
            _controller.MerchantInputManager.ExecuteEvent += SendItemsRequested;
            _controller.MerchantInputManager.ResetEvent += ResetTransferRequested;
            _controller.MerchantInputManager.InteractEvent += ViewCharacterDetails;

            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
        }

        private void GetInGameCharacters(GetInGameHeroesSucceeded obj)
        {
            ConvertAndPassDataToUi(obj.Heroes, _cachedGameData, _controller.UIGameList);
            _controller.UIGameList.HandleInPartyHeroes();
        }

        private void GetInDboxCharacters(GetInDboxHeroesSucceeded obj) =>
            ConvertAndPassDataToUi(obj.Heroes, _cachedDboxData, _controller.UIDboxList);

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

            _controller.UIGameList.SetData(_cachedGameData);
            _controller.UIDboxList.SetData(_cachedDboxData);
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