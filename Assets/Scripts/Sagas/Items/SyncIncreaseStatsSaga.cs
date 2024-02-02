using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay;

namespace CryptoQuest.Sagas.Items
{
    public class SyncStatsAction : ActionBase
    {
        public HeroBehaviour Character { get; private set; }
        public AttributeWithValue[] AttributeValues { get; private set; }
        public SyncStatsAction(HeroBehaviour character, params AttributeWithValue[] attributeValues)
        {
            Character = character;
            AttributeValues = attributeValues;
        }
    }

    public class SyncIncreaseStatsSaga : SagaBase<SyncStatsAction>
    {
        [SerializeField] private List<ResponseAttributeMap> _attributeMap = new();

        private Dictionary<AttributeScriptableObject, string> _attributeMapDict = new();
        private Dictionary<object, object> _requestBody = new();

        private void Awake()
        {
            foreach (var attributeMap in _attributeMap)
            {
                _attributeMapDict.TryAdd(attributeMap.Attribute, attributeMap.Name);
            }
        }

        protected override void HandleAction(SyncStatsAction ctx)
        {
            StartCoroutine(CoSyncToServer(ctx));
        }

        private IEnumerator CoSyncToServer(SyncStatsAction ctx)
        {
            ComposeBody(ctx);

            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(_requestBody)
                .Request<CharactersResponse>(ERequestMethod.PUT, CharacterAPI.CHARACTERS)
                .ToYieldInstruction();
            yield return op;
            if (op.HasError)
            {
                Debug.LogError($"SyncIncreaseStats::CoSyncToServer: Failed to sync stats to server: {op.Error}");
                yield break;
            }
        }

        private void ComposeBody(SyncStatsAction ctx)
        {
            _requestBody.Clear();
            _requestBody.Add("id", ctx.Character.Spec.Id);
            List<ModifyStat> modifyStats = new();
            foreach (var attributeValue in ctx.AttributeValues)
            {
                if (!_attributeMapDict.TryGetValue(attributeValue.Attribute, out var attributeName))
                    continue;

                var existValue = 0f;
                var existAttribute = new CappedAttributeDef();
                foreach (var attribute in ctx.Character.Stats.Attributes)
                {
                    if (attribute.Attribute == attributeValue.Attribute)
                    {
                        existAttribute = attribute;
                        break;
                    }
                } 
                if (existAttribute.Attribute != null)
                {
                    existValue = existAttribute.ModifyValue;
                    existAttribute.ModifyValue += attributeValue.Value;
                }
                _requestBody.Add(attributeName, existValue + attributeValue.Value);
            }
        }
    }
}