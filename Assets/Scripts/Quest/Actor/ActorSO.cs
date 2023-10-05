using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Quest.Actor
{
    public abstract class ActorSO : SerializableScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public QuestSO QuestData { get; private set; }
        [field: SerializeField] public AssetReference Prefab { get; private set; }
        public abstract ActorInfo CreateActor(QuestManager questManager);
    }

    public abstract class ActorSO<TDef> : ActorSO where TDef : ActorInfo, new()
    {
        public override ActorInfo CreateActor(QuestManager questManager) => new TDef();
    }
}