using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;

namespace CryptoQuest.Battle
{
    public struct EnemyGroup
    {
        public EnemyDef Def;
        public List<EnemyBehaviour> Enemies;
        public int Count => Enemies.Count;
    }
}