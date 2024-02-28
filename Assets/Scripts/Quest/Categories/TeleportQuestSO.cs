﻿using CryptoQuest.System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.Common;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Teleport Quest", fileName = "TeleportQuestSO")]
    public class TeleportQuestSO : QuestSO
    {
        public SceneScriptableObject Destination;

        public override QuestInfo CreateQuest()
            => new TeleportQuestInfo(this);
    }

    public class TeleportQuestInfo : QuestInfo<TeleportQuestSO>
    {
        public TeleportQuestInfo(TeleportQuestSO teleportQuestSo) : base(teleportQuestSo)
        {
        }

        public override void TriggerQuest()
        {
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            var questManager = ServiceProvider.GetService<IQuestManager>();
            var questTeleportController = questManager?.GetComponent<QuestTeleportController>();
            questTeleportController?.GiveQuest(this);
        }
    }
}