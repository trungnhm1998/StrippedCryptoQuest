using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Quest.Dialogue
{
    public interface IDialogueDef
    {
        public int LinesCount { get; }
        public LocalizedString SpeakerName { get; }
        public LocalizedString GetLine(int lineIndex);
    }
}