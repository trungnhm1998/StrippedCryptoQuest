﻿using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    [CreateAssetMenu(menuName = "Create BattleBus Skill Test", fileName = "BattleBus", order = 0)]
    public class MockBattleSkillBus : BattleBus
    {
        public SkillName[] Skill = new[]
        {
            new SkillName()
            {
                name = "bantumlum",
            },
            new SkillName()
            {
                name = "oibanoi",
            },
            new SkillName()
            {
                name = "nigga",
            }
        };
    }
}