using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Actions;
using CryptoQuest.Events.UI;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using TinyMessenger;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Sagas.MagicStone
{
    public class GetMagicStones : MonoBehaviour
    {
        [SerializeField] private MagicStoneDefinitionDatabase _magicStoneDefinitionDatabase;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
        [SerializeField] private MagicStoneInventorySo _stoneInventory;

        private TinyMessageSubscriptionToken _fetchEvent;
        private TinyMessageSubscriptionToken _transferSuccessEvent;

        private MagicStoneResponseConverter _converter;

        private void Awake()
        {
            _converter = new MagicStoneResponseConverter(_magicStoneDefinitionDatabase, _passiveAbilityDatabase);
        }

        private void OnEnable()
        {
            _fetchEvent = ActionDispatcher.Bind<FetchProfileMagicStonesAction>(HandleAction);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_fetchEvent);
        }

        private void HandleAction(FetchProfileMagicStonesAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<MagicStonesResponse>(API.Profile.MAGIC_STONE)
                .Subscribe(ProcessResponseCharacters, OnError);
        }

        private void ProcessResponseCharacters(MagicStonesResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;
            var responseStones = obj.data.stones;
            if (responseStones.Length == 0) return;
            OnInventoryFilled(responseStones);
            FilterDboxStones(responseStones);
        }

        private void FilterDboxStones(Objects.MagicStone[] stonesResponse)
        {
            if (stonesResponse.Length == 0) return;
            var dboxStones = stonesResponse.Where(stone => stone.inGameStatus == (int)Objects.EMagicStoneStatus.InBox).ToList();
            ActionDispatcher.Dispatch(new FetchDBoxMagicStonesSucceeded(dboxStones));
        }

        private void OnInventoryFilled(Objects.MagicStone[] stonesResponse)
            => StartCoroutine(CoLoadAndUpdateInventory(stonesResponse));

        private IEnumerator CoLoadAndUpdateInventory(Objects.MagicStone[] stonesResponse)
        {
            _stoneInventory.MagicStones.Clear();
            foreach (var stoneResponse in stonesResponse)
            {
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId1);
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId2);
                _stoneInventory.MagicStones.Add(_converter.Convert(stoneResponse));
            }
            ActionDispatcher.Dispatch(new StoneInventoryFilled());
        }

        private void OnError(Exception error)
        {
            Debug.LogError("FetchProfileCharacters::OnError " + error);
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new FetchMagicStonesFailed());
        }
    }
}