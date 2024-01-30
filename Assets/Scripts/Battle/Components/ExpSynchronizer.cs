using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class ExpSynchronizer : CharacterComponentBase
    {
        private IExpProvider _expProvider;
        private LevelSystem _levelSystem;
        private IHeroSpecProvider _specProvider;

        protected override void OnAwake()
        {
            Character.TryGetComponent(out _specProvider);
            Character.TryGetComponent(out _expProvider);
            Character.TryGetComponent(out _levelSystem);
        }

        protected override void OnInit()
        {
            base.OnInit();
            _levelSystem.ExpAdded += SyncToServer;
        }

        protected override void OnReset()
        {
            base.OnReset();
            _levelSystem.ExpAdded -= SyncToServer;
        }

        private void SyncToServer()
        {
            StartCoroutine(CoSyncToServer());
        }

        private IEnumerator CoSyncToServer()
        {
            float exp = _expProvider.Exp;
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(new { id = _specProvider.Spec.Id, exp })
                .Request<CharactersResponse>(ERequestMethod.PUT, CharacterAPI.CHARACTERS)
                .ToYieldInstruction();
            yield return op;
            if (op.HasError)
            {
                Debug.LogError($"ExpSynchronizer::CoSyncToServer: Failed to sync exp to server: {op.Error}");
                yield break;
            }
        }
    }
}