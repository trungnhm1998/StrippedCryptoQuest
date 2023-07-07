using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject
{
    [CreateAssetMenu(menuName = "Crypto Quest/Story/Dialogue")]
    public class DialogueScriptableObject : UnityEngine.ScriptableObject
    {
        [SerializeField] private List<LocalizedString> _lines;

        public int LinesCount => _lines.Count;

        public LocalizedString GetLine(int currentIndex)
        {
            return _lines[currentIndex];
        }
    }
}