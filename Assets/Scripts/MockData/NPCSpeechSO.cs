using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.MockData
{
    [CreateAssetMenu(menuName = "MockData/NPC Speech Data")]
    public class NPCSpeechSO : ScriptableObject
    {
        [TextArea(5, 10)]
        public string Message;
    }
}