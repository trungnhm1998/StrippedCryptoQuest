using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Sagas.MagicStone;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Item.MagicStone
{
    [TestFixture]
    public class ResponseConverterTests
    {
        private IMagicStoneResponseConverter _provider;
        private GameObject _prefab;
        private PassiveAbilityDatabase _passiveDatabase;

        private bool _hasSetup = false;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _passiveDatabase = AssetDatabase.LoadAssetAtPath<PassiveAbilityDatabase>(
                "Assets/ScriptableObjects/Character/Skills/PassiveAbilityDatabase.asset");
            _prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Providers/MagicStoneProvider.prefab");
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            if (_hasSetup) yield break;
            _hasSetup = true;

            // preload
            var instance = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;
            Assert.NotNull(instance, "Prefab instance");
            _provider = instance.GetComponent<IMagicStoneResponseConverter>();
        }


        [UnityTest]
        public IEnumerator Convert_WithResponse_ShouldReturnMagicStone()
        {
            var response =
                "{\"id\":38,\"stoneTokenId\":\"\",\"userId\":\"OVFUSTxqSOdCW1cdg7E41Fzi2Yk1\",\"walletAddress\":\"\",\"stoneId\":\"4503111\",\"attachEquipment\":0,\"inGameStatus\":2,\"mintStatus\":0,\"transferring\":0,\"createdAt\":\"2023-12-01T11:04:01.000Z\",\"updatedAt\":\"2023-12-01T11:04:01.000Z\",\"stoneNameEn\":\"WindMagicStone\",\"stoneNameJp\":\"風の魔石\",\"element\":\"Wind\",\"elementId\":\"5\",\"stoneLv\":3,\"afterUpgradeStoneId\":\"4504111\",\"skillType\":111,\"passiveSkillId1\":4083,\"passiveSkillId2\":4183,\"price\":0,\"sellingPrice\":0}";
            var responseObject = JsonConvert.DeserializeObject<Sagas.Objects.MagicStone>(response);

            var magicStone = _provider.Convert(responseObject);

            yield return new WaitUntil(() => magicStone.Passives != null && magicStone.Passives.Length > 0);

            Assert.AreEqual(responseObject.id, magicStone.ID, "Id");
            Assert.AreEqual(responseObject.stoneLv, magicStone.Level, "level");
            Assert.NotNull(magicStone.Definition, "Has definition");
            Assert.AreEqual(responseObject.elementId, magicStone.Definition.ID, "definition id");
            Assert.AreEqual(2, magicStone.Passives.Length, "Number of skills");
            Assert.AreEqual(responseObject.passiveSkillId1, magicStone.Passives[0].Id, "First Skill id");
            Assert.AreEqual(responseObject.passiveSkillId2, magicStone.Passives[1].Id, "Second SKill id");
        }
    }
}