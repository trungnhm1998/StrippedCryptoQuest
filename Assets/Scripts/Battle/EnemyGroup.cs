using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;

namespace CryptoQuest.Battle
{
    public struct EnemyGroup
    {
        public EnemyDef Def;
        public List<EnemyBehaviour> Enemies;
        public List<EnemySpec> EnemySpecs;
        public int Count => Enemies.Where(e => e.IsValid()).Count();

        public void Init()
        {
            Enemies = new();
            EnemySpecs = new();
        }

        public bool IsValid()
        {
            return Def != null;
        }

        public List<EnemyBehaviour> GetAliveEnemies()
        {
            return Enemies.Where(e => e.IsValid()).ToList();
        }
    }
}