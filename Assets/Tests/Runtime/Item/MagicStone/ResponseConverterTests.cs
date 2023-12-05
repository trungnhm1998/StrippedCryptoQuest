using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Item.MagicStone
{
    [TestFixture]
    public class ResponseConverterTests
    {
        private MagicStoneDefinitionDatabase _database;
        private PassiveAbilityDatabase _passiveDatabase;

        private bool _hasSetup = false;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _database = AssetDatabase.LoadAssetAtPath<MagicStoneDefinitionDatabase>(
                "Assets/ScriptableObjects/MagicStones/MagicStoneDefinitionDatabase.asset");
            _passiveDatabase = AssetDatabase.LoadAssetAtPath<PassiveAbilityDatabase>(
                "Assets/ScriptableObjects/Character/Skills/PassiveAbilityDatabase.asset");
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            if (_hasSetup) yield break;
            _hasSetup = true;

            // preload
            foreach (var skill in _passiveDatabase.CacheLookupTable)
            {
                yield return _passiveDatabase.LoadDataById(skill.Key);
            }

            // yield return _passiveDatabase.LoadDataByIdAsync(4071);
            // yield return _passiveDatabase.LoadDataByIdAsync(4181);
            // yield return _passiveDatabase.LoadDataByIdAsync(4083);
            // yield return _passiveDatabase.LoadDataByIdAsync(4183);
        }

        [TestCase(
            "{\"id\":37,\"stoneTokenId\":\"\",\"userId\":\"OVFUSTxqSOdCW1cdg7E41Fzi2Yk1\",\"walletAddress\":\"\",\"stoneId\":\"4401111\",\"attachEquipment\":0,\"inGameStatus\":2,\"mintStatus\":0,\"transferring\":0,\"createdAt\":\"2023-12-01T11:03:52.000Z\",\"updatedAt\":\"2023-12-01T11:03:52.000Z\",\"stoneNameEn\":\"EarthMagicStone\",\"stoneNameJp\":\"地の魔石\",\"element\":\"Earth\",\"elementId\":\"4\",\"stoneLv\":1,\"afterUpgradeStoneId\":\"4402111\",\"skillType\":111,\"passiveSkillId1\":4071,\"passiveSkillId2\":4181,\"price\":0,\"sellingPrice\":0}")]
        [TestCase(
            "{\"id\":38,\"stoneTokenId\":\"\",\"userId\":\"OVFUSTxqSOdCW1cdg7E41Fzi2Yk1\",\"walletAddress\":\"\",\"stoneId\":\"4503111\",\"attachEquipment\":0,\"inGameStatus\":2,\"mintStatus\":0,\"transferring\":0,\"createdAt\":\"2023-12-01T11:04:01.000Z\",\"updatedAt\":\"2023-12-01T11:04:01.000Z\",\"stoneNameEn\":\"WindMagicStone\",\"stoneNameJp\":\"風の魔石\",\"element\":\"Wind\",\"elementId\":\"5\",\"stoneLv\":3,\"afterUpgradeStoneId\":\"4504111\",\"skillType\":111,\"passiveSkillId1\":4083,\"passiveSkillId2\":4183,\"price\":0,\"sellingPrice\":0}")]
        public void Convert_WithResponse_ShouldReturnMagicStone(string response)
        {
            var responseObject = JsonConvert.DeserializeObject<Sagas.Objects.MagicStone>(response);

            var converter = new MagicStoneResponseConverter(_database, _passiveDatabase);
            CryptoQuest.Item.MagicStone.IMagicStone magicStone = converter.Convert(responseObject);

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