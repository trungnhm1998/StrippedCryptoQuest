using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas;
using CryptoQuest.System.SaveSystem.Savers;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class ClearProfileAction : ActionBase
    {
    }

    public class ClearProfileRequestHandler : CoSagaBase<ClearProfileAction>
    {
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private Credentials _credentials;

        protected override IEnumerator HandleActionCoroutine(ClearProfileAction ctx)
        {
            var restClient = ServiceProvider.GetService<IRestClient>();

            var newSaveData = new SaveData();

            var op = restClient
                .WithoutDispactError()
                .WithBody(new OnlineProgressionSaver.SaveDataBody() { GameData = newSaveData })
                .Request<OnlineProgressionSaver.SaveDataResult>(ERequestMethod.POST, Accounts.USER_SAVE_DATA)
                .ToYieldInstruction();

            yield return op;

            _saveSystem.SaveData = newSaveData;
            _saveSystem.Save(); // this will override the localsave

            ActionDispatcher.Dispatch(new GoToTitleAction());
        }
    }
}