using System;
using System.Collections.Generic;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [CreateAssetMenu(fileName = "YarnDialogQuest", menuName = "QuestSystem/Yarn/Yarn Dialog Quest Node")]
    public class YarnDialogWithQuestSo : ScriptableObject
    {
        public YarnQuestDef YarnQuestDef;
    }

    [Serializable]
    public class YarnQuestDef
    {
        public string YarnNode;
        public YarnProjectConfigSO YarnProjectConfig;
        public List<QuestSO> PossibleOutcomeQuests;
    }
}