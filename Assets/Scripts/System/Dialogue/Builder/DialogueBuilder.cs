using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;

namespace CryptoQuest.System.Dialogue.Builder
{
    public class DialogueBuilder
    {
        private readonly List<LocalizedString> _localizedLines = new();

        public DialogueBuilder WithLines(params LocalizedString[] localizedLines)
        {
            _localizedLines.AddRange(localizedLines);
            return this;
        }

        public Dialogue Build()
        {
            // is it safer to return a copy of the list?
            return new Dialogue(_localizedLines);
        }

        public static implicit operator Dialogue(DialogueBuilder builder) => builder.Build();
    }
}