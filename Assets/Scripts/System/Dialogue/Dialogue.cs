using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Quest.Dialogue;
using UnityEngine.Localization;

namespace CryptoQuest.System.Dialogue
{
    [Serializable]
    public class Dialogue : IDialogueDef
    {
        private List<LocalizedString> _lines;
        private LocalizedString _speakerName = new LocalizedString();

        public Dialogue(List<LocalizedString> lines, LocalizedString speakerName = default)
        {
            _lines = lines;
        }

        public int LinesCount => _lines.Count;
        public LocalizedString SpeakerName => _speakerName;

        public LocalizedString GetLine(int lineIndex) => _lines[lineIndex];
    }
}