using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class AbsorbingLogger : LoggerComponentBase
    {
        [Serializable]
        private struct AttributeToName
        {
            public AttributeScriptableObject Attribute;
            public LocalizedString Name;
        }

        [SerializeField] List<AttributeToName> _attributesToName;
        [SerializeField] private LocalizedString _localized;
        private readonly Dictionary<AttributeScriptableObject, LocalizedString> _attributesToNameDict = new();
        private TinyMessageSubscriptionToken _absorbingEvent;

        private void Awake()
        {
            foreach (var attr in _attributesToName)
                _attributesToNameDict.Add(attr.Attribute, attr.Name);
            _absorbingEvent = BattleEventBus.SubscribeEvent<AbsorbingEvent>(LogAbsorbing);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_absorbingEvent);
        }

        private void LogAbsorbing(AbsorbingEvent ctx)
        {
            var msg = new LocalizedString(_localized.TableReference, _localized.TableEntryReference);
            msg.Add(Constants.NUMBER, new IntVariable { Value = Mathf.CeilToInt(ctx.Value) });
            msg.Add(Constants.ATTRIBUTE_NAME, _attributesToNameDict[ctx.AbsorbingAttribute]);
            msg.Add(Constants.CHARACTER_NAME, new StringVariable() { Value = ctx.Character.DisplayName });
            
            Logger.QueueLog(msg);
        }
    }
}