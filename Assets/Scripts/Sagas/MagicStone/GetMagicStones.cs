using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CryptoQuest.AbilitySystem.Abilities;
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
    public class GetMagicStones : SagaBase<FetchProfileMagicStonesAction>
    {
        [SerializeField] private MagicStoneDefinitionDatabase _magicStoneDefinitionDatabase;
        [SerializeField] private PassiveAbilityDatabase _passiveAbilityDatabase;
        [SerializeField] private MagicStoneInventory _stoneInventory;

        private IMagicStoneResponseConverter _converter;

        private void Awake()
        {
            _converter = ServiceProvider.GetService<IMagicStoneResponseConverter>();
        }

        protected override void HandleAction(FetchProfileMagicStonesAction _)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient
                .Get<MagicStonesResponse>(API.Profile.MAGIC_STONE)
                .Subscribe(ProcessResponseMagicStone, OnError);
        }

        private void ProcessResponseMagicStone(MagicStonesResponse obj)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            if (obj.code != (int)HttpStatusCode.OK) return;

            var responseStones = obj.data.stones;
            if (responseStones.Length == 0) return;

            FilterStones(responseStones, EMagicStoneStatus.InBox);
            FilterStones(responseStones, EMagicStoneStatus.InGame);

            ActionDispatcher.Dispatch<GetStonesResponsed>(new GetStonesResponsed(obj));
        }

        private void FilterStones(Objects.MagicStone[] stonesResponse, EMagicStoneStatus status)
        {
            if (stonesResponse.Length == 0) return;

            var filteredStones = stonesResponse
                .Where(stone => stone.inGameStatus.Equals((int)status))
                .ToList();

            var filteredStonesList = new List<IMagicStone>() { NullMagicStone.Instance };
            filteredStones.ForEach(stone => filteredStonesList.Add(_converter.Convert(stone)));

            switch (status)
            {
                case EMagicStoneStatus.InBox:
                    ActionDispatcher.Dispatch(new FetchInboxMagicStonesSuccess(filteredStones.ToArray()));
                    break;
                case EMagicStoneStatus.InGame:
                default:
                    OnInventoryFilled(filteredStones.ToArray());
                    ActionDispatcher.Dispatch(new FetchIngameMagicStonesSuccess(filteredStones.ToArray()));
                    break;
            }
        }

        private void OnInventoryFilled(Objects.MagicStone[] stonesResponse)
            => StartCoroutine(CoLoadAndUpdateInventory(stonesResponse));

        private IEnumerator CoLoadAndUpdateInventory(Objects.MagicStone[] stonesResponse)
        {
            _stoneInventory.MagicStones.Clear();
            foreach (var stoneResponse in stonesResponse)
            {
                if (stoneResponse.id == -1) continue;
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId1);
                yield return _passiveAbilityDatabase.LoadDataById(stoneResponse.passiveSkillId2);
                _stoneInventory.MagicStones.Add(_converter.Convert(stoneResponse));
            }

            ActionDispatcher.Dispatch(new StoneInventoryFilled());
        }

        private void OnError(Exception error)
        {
            Debug.LogWarning("FetchProfileCharacters::OnError " + error);
            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new ServerErrorPopup());
            ActionDispatcher.Dispatch(new GetStonesFailed());
        }
    }
}