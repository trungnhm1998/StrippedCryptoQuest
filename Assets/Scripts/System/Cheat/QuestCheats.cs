using System.Collections.Generic;
using System.Linq;
using CommandTerminal;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class QuestCheats : MonoBehaviour, ICheatInitializer
    {
        [field: SerializeField] public QuestDatabase QuestDatabase { get; private set; }

        private Dictionary<string, AbstractObjective> _questDictionary
        {
            get { return QuestDatabase.Quests.ToDictionary(quest => quest.name.ToLower(), value => value); }
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("complete", CompleteQuest, 1, 1, "Complete a quest");
        }

        private void CompleteQuest(CommandArg[] args)
        {
            var questName = args[0].String.ToLower();
            if (!_questDictionary.ContainsKey(questName))
            {
                Debug.LogWarning($"Quest {questName} not found");
                return;
            }

            _questDictionary[questName].OnComplete();
        }
    }
}