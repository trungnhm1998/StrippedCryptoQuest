using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [CreateAssetMenu(fileName = "YarnDialogQuest", menuName = "Quest System/Yarn Dialog Quest")]
    public class YarnDialogWithQuestSo : ScriptableObject
    {
        public YarnQuestDef YarnQuestDef;
    }

    [Serializable]
    public class YarnQuestDef
    {
        public string YarnNode;
        public List<QuestSO> PossibleOutcomeQuests;
    }
}