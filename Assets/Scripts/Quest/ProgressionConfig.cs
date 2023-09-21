using System;
using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    [Serializable]
    public class ProgressionConfig
    {
        public Authoring.Quest TargetQuest;
        public Task TargetTask;
    }
}