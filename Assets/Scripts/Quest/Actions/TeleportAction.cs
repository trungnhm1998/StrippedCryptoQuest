using System.Collections;
using CryptoQuest.Map;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "Quest/Actions/Teleport")]
    public class TeleportAction : NextAction
    {
        public QuestEventChannelSO GiveQuestEventChannel;
        public QuestSO QuestToGive;
        [Header("Configs")] [SerializeField] private SceneScriptableObject _nextScene;

        [SerializeField] private MapPathSO _mapPath;

        [Header("Refs")] [SerializeField] private LoadSceneEventChannelSO _loadNextSceneEventChannelSO;

        [SerializeField] private PathStorageSO _transitionSO;

        public override IEnumerator Execute()
        {
            if (QuestToGive != null)
                GiveQuestEventChannel.RaiseEvent(QuestToGive);
            OnTriggerTeleport(_nextScene);
            yield break;
        }

        private void OnTriggerTeleport(SceneScriptableObject scene)
        {
            UpdateTransitionName(_mapPath);
            _loadNextSceneEventChannelSO.RequestLoad(scene);
        }

        private void UpdateTransitionName(MapPathSO transitionPath)
        {
            _transitionSO.LastTakenPath = transitionPath;
        }
    }
}