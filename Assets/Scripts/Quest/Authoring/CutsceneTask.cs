using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(menuName = "Quest System/Stages/Cutscene", fileName = "CutsceneStage")]
    public class CutsceneTask : Task
    {
        /// <summary>
        /// Try to play the cut scene as soon as the scene is loaded
        /// </summary>
        public bool PlayOnLoaded = true;
    }
}