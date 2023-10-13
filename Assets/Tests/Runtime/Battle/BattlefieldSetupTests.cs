using System.IO;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Core.Events.ScriptableObjects;
using NUnit.Framework;
using Spine.Unity;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class BattlefieldSetupTests
    {
        private static readonly WaitForSeconds SecondsToWait = new(7);
        private const string STARTUP_SCENE_PATH = "Assets/Scenes/WIP/PlayModeTestScene.unity";
        private const string BATTLEFIELD_ASSETS_PATH = "Assets/ScriptableObjects/Battlefields/";

        private const string BATTLE_END_EVENT_SO_PATH =
            "Assets/ScriptableObjects/Battle/Events/BattleEndEventChannel.asset";

        private Dictionary<int, Battlefield> _battlefields1k = new();
        private Dictionary<int, Battlefield> _battlefields2k = new();
        private Dictionary<int, Battlefield> _battlefields3k = new();
        private Dictionary<int, Battlefield> _battlefields4k = new();
        private Dictionary<int, Battlefield> _battlefields5k = new();
        private Dictionary<int, Battlefield> _battlefieldsRemain = new();
        private VoidEventChannelSO _battleEndEventChannel;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _battlefields1k = new();
            _battlefields2k = new();
            _battlefields3k = new();
            _battlefields4k = new();
            _battlefields5k = new();
            _battlefieldsRemain = new();
            SetUpEventChannel();
            var info = new DirectoryInfo(BATTLEFIELD_ASSETS_PATH);
            var fileInfos = info.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                var asset =
                    AssetDatabase.LoadAssetAtPath(BATTLEFIELD_ASSETS_PATH + fileInfo.Name, typeof(Battlefield));

                if (asset is Battlefield battlefield)
                {
                    var firstLetter = battlefield.Id.ToString().Substring(0, 1);
                    switch (firstLetter)
                    {
                        case "1":
                            _battlefields1k.TryAdd(battlefield.Id, battlefield);
                            break;
                        case "2":
                            _battlefields2k.TryAdd(battlefield.Id, battlefield);
                            break;
                        case "3":
                            _battlefields3k.TryAdd(battlefield.Id, battlefield);
                            break;
                        case "4":
                            _battlefields4k.TryAdd(battlefield.Id, battlefield);
                            break;
                        case "5":
                            _battlefields5k.TryAdd(battlefield.Id, battlefield);
                            break;
                        default:
                            _battlefieldsRemain.TryAdd(battlefield.Id, battlefield);
                            break;
                    }
                }
            }

            EditorSceneManager.LoadSceneAsyncInPlayMode(STARTUP_SCENE_PATH,
                new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return new WaitForSeconds(4);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_1000s_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefields1k);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_2000s_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefields2k);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_3000s_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefields3k);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_4000s_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefields4k);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_5000s_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefields5k);
        }

        [UnityTest]
        [Timeout(100000000)]
        public IEnumerator BattlefieldSetup_Remain_ReturnCorrectEnemyCount()
        {
            yield return BattleFieldTest(_battlefieldsRemain);
        }

        private IEnumerator BattleFieldTest(Dictionary<int, Battlefield> battlefields)
        {
            foreach (var battlefield in battlefields)
            {
                yield return new WaitForSeconds(2);
                BattleLoader.RequestLoadBattle(battlefield.Key);
                yield return SecondsToWait;
                Debug.Log("test case: " + battlefield.Value.Id);
                var enemyPartyGameObject = GameObject.Find("EnemyParty");
                EnemyPartyBehaviour enemyParty = enemyPartyGameObject.GetComponent<EnemyPartyBehaviour>();
                int monsterCount = ValidMonsterCount(enemyParty.Enemies);
                Assert.AreEqual(battlefield.Value.EnemyIds.Length, monsterCount);
                _battleEndEventChannel.RaiseEvent();
            }
        }

        private void SetUpEventChannel()
        {
            _battleEndEventChannel = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(BATTLE_END_EVENT_SO_PATH);
        }

        private int ValidMonsterCount(List<EnemyBehaviour> enemies)
        {
            int count = 0;
            foreach (var enemy in enemies)
            {
                var result = enemy.GetComponentInChildren<SkeletonAnimation>();
                count += result != null ? 1 : 0;
            }

            return count;
        }
    }
}