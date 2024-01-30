using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroRuntimeStatsSynchronizer : CharacterComponentBase
    {
        [SerializeField] private AttributeScriptableObject[] _attributesToSync;

        private HashSet<AttributeScriptableObject> _attributeScriptableObjectSet;

        private IHeroSpecProvider _heroSpecProvider;

        protected override void Awake()
        {
            base.Awake();
            TryGetComponent(out _heroSpecProvider);
            _attributeScriptableObjectSet = new HashSet<AttributeScriptableObject>(_attributesToSync);
        }

        protected override void OnInit()
        {
            base.OnInit();
            Character.AttributeSystem.PostAttributeChange += OnAttributeChange;
        }

        protected override void OnReset()
        {
            base.OnReset();
            Character.AttributeSystem.PostAttributeChange -= OnAttributeChange;
        }

        private void OnAttributeChange(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue)
        {
            if (Mathf.Approximately(oldValue.CurrentValue, newValue.CurrentValue)) return;
            if (!_attributeScriptableObjectSet.Contains(attribute)) return;

            var heroSpec = _heroSpecProvider.Spec;
            var runtimeStats = heroSpec.RuntimeStats;
            for (int i = 0; i < runtimeStats.Count; i++)
            {
                var attributeWithValue = runtimeStats[i];
                if (attributeWithValue.Attribute != attribute) continue;
                attributeWithValue.Value = newValue.CurrentValue;
                runtimeStats[i] = attributeWithValue;
                _heroSpecProvider.Spec.RuntimeStats = runtimeStats;

                StartCoroutine(CoSyncToServer(attribute, newValue.CurrentValue));

                return;
            }

            StartCoroutine(CoSyncToServer(attribute, newValue.CurrentValue));
            runtimeStats.Add(new AttributeWithValue(attribute, newValue.CurrentValue));
            _heroSpecProvider.Spec.RuntimeStats = runtimeStats;
        }

        private IEnumerator CoSyncToServer(AttributeScriptableObject attribute, float newValue)
        {
            var body = GetBody(attribute, newValue);
            body.Id = _heroSpecProvider.Spec.Id;
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithBody(body)
                .Request<CharactersResponse>(ERequestMethod.PUT, CharacterAPI.CHARACTERS).ToYieldInstruction();

            yield return op;

            if (op.HasError)
            {
                Debug.LogError($"Error when sync {attribute} to server: {op.Error}");
                yield break;
            }

            Debug.Log($"Sync {attribute} to server success");
        }

        private Body GetBody(AttributeScriptableObject attribute, float newValue)
        {
            if (attribute == AttributeSets.Mana)
            {
                return new MpBody()
                {
                    Mp = newValue
                };
            }

            if (attribute == AttributeSets.Health)
            {
                return new HpBody()
                {
                    Hp = newValue
                };
            }

            return new Body();
        }

        [Serializable]
        class Body
        {
            [JsonProperty("id")]
            public int Id;
        }

        [Serializable]
        class MpBody : Body
        {
            [JsonProperty("MP")]
            public float Mp;
        }

        [Serializable]
        class HpBody : Body
        {
            [JsonProperty("HP")]
            public float Hp;
        }
    }
}