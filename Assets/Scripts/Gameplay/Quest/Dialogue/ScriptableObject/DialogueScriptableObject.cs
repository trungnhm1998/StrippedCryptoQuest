using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject
{
    [CreateAssetMenu(menuName = "Crypto Quest/Story/Dialogue")]
    public class DialogueScriptableObject : UnityEngine.ScriptableObject, IDialogueDef
    {
        [SerializeField] private List<LocalizedString> _lines;

        [SerializeField] private LocalizedString _npcName;

        public int LinesCount => _lines.Count;
        public LocalizedString SpeakerName => _npcName;

        public LocalizedString GetLine(int lineIndex) => _lines[lineIndex];
    }
}