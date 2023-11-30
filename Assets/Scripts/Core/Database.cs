using System;
using System.IO;
using CryptoQuest.Character;
using CryptoQuest.Character.Skill;
using CryptoQuest.System;
using SQLite;
using UnityEngine;

namespace CryptoQuest.Core
{
    public class Database : MonoBehaviour
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        class BaseTable
        {
            [PrimaryKey]
            public int Id { get; set; }

            public string Guid { get; set; }
        }

        class Skills : BaseTable { }

        class CharacterClasses : BaseTable { }

        class Elements : BaseTable { }


        public event Action Initialized;
        private SQLiteConnection _connection;
        public string DatabaseName = "CryptoQuest.sqlite";

        private void Awake()
        {
            ServiceProvider.Provide(this);

#if UNITY_EDITOR
            var path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, DatabaseName);
#else
            var path = Path.Combine(Application.dataPath, DatabaseName);
#endif
            _connection = new SQLiteConnection(path);
            _connection.CreateTable<Skills>();
            _connection.CreateTable<CharacterClasses>();
            _connection.CreateTable<Elements>();
            _connection.CreateTable<HeroSkillsSet>();
            _connection.CreateIndex(nameof(HeroSkillsSet), new[] { "Class", "Element", "Skill" });

            Initialized?.Invoke();
        }

        private void OnApplicationQuit()
        {
            _connection?.Close();
        }
#endif
    }
}