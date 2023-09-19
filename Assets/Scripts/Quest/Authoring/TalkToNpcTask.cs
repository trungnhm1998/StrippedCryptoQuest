using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(menuName = "Quest System/Stages/Talk to NPC", fileName = "TalkToNpcStage", order = 0)]
    public class TalkToNpcTask : Task
    {
        [field: SerializeField] public string YarnNode { get; private set; }
    }
}