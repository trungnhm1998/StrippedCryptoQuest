using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(menuName = "QuestSystem/Chainable Action Node/PlayCutsceneNode")]
    public class PlayCutsceneChainableNode : ActionChainableNodeSO
    {
        [SerializeField] private QuestCutsceneDef _cutscene;

        public override void Execute()
        {
            CutsceneManager.CutsceneCompleted += ExecuteNextNode;
            _cutscene.RaiseEvent();
        }

        protected override void ExecuteNextNode()
        {
            CutsceneManager.CutsceneCompleted -= ExecuteNextNode;
            base.ExecuteNextNode();
        }
    }
}