using System.Collections;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.Sagas.Profile;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Profile
{
    [TestFixture, Category("Smokes")]
    public class EquipmentResponseConverterTests
    {
        private IEquipmentResponseConverter _responseConverter;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var converter = AssetDatabase.LoadAssetAtPath<EquipmentResponseConverter>(
                "Assets/Prefabs/Providers/EquipmentProvider.prefab");
            _responseConverter = Object.Instantiate(converter);
        }

        [UnityTest]
        public IEnumerator Converter_ShouldLoadDataCorrect()
        {
            var json =
                "{\"id\":62,\"equipmentTokenId\":\"519\",\"userId\":\"zeYhVkERAAgODkQp75Y728Tfd6V2\",\"walletAddress\":\"0xe95620f7fff06812ce318a6d22d30290fb556b79\",\"lv\":1,\"equipmentId\":\"201015000421\",\"equipTypeId\":101,\"attack\":552,\"addAttack\":3,\"MATK\":0,\"addMATK\":0,\"defence\":0,\"addDefence\":0,\"criticalRate\":0,\"evasionRate\":0,\"attachUnitTokenId\":\"\",\"inGameStatus\":2,\"mintStatus\":2,\"transferring\":0,\"createdAt\":\"2023-11-15T10:41:15.000Z\",\"updatedAt\":\"2023-11-15T10:41:15.000Z\",\"equipmentIdForeign\":\"10290\",\"localizeKey\":\"DAGGER_ULTIMATEBREAKER\",\"equipNameJp\":\"アルティメットブレイカー\",\"equipTypeNameJp\":\"短剣\",\"seriesNameJp\":\"\",\"equipName\":\"UltimateBreaker\",\"equipTypeName\":\"Dagger\",\"rarityName\":\"legend\",\"categoryId\":\"0\",\"seriesName\":\"\",\"equipPartId\":\"1\",\"rarityId\":50,\"seriesId\":\"\",\"groupId\":\"10150004\",\"star\":2,\"evoLv\":0,\"restrictedLv\":0,\"price\":0,\"sellingPrice\":0,\"minLv\":10,\"maxLv\":20,\"minAttack\":549,\"minMATK\":0,\"minDefence\":0,\"minEvasionRate\":0,\"minCriticalRate\":0,\"maxAttack\":699,\"maxMATK\":0,\"maxDefence\":0,\"maxEvasionRate\":0,\"maxCriticalRate\":0,\"miningPower\":2413,\"consumeFuel\":2413,\"imageFileName\":\"NFT-WA_91-01\",\"passiveSkillId1\":4004,\"passiveSkillId2\":0,\"conditionSkillId\":0,\"nft\":1,\"slot\":3,\"imageURL\":\"https://bafybeigi2gcr3fpy22tv3u32ewu5goe23u3544nteadyjtts65zvmj24ui.ipfs.nftstorage.link/NFT-WA_91-01.png\",\"randomNumberBonus\":9,\"valuePerLv\":15,\"attachStones\":{},\"passiveSkills\":[4004],\"conditionSkills\":[]}";
            var equipmentResponse = JsonUtility.FromJson<EquipmentResponse>(json);
            var equipment = _responseConverter.Convert(equipmentResponse);

            Assert.NotNull(equipment.Stats);
            Assert.AreNotEqual(0, equipment.Stats.Length);
            Assert.AreEqual(equipmentResponse.id, equipment.Id);
            Assert.AreEqual(equipmentResponse.equipmentId, equipment.Data.ID);
            Assert.AreEqual(equipmentResponse.star, equipment.Data.Stars);
            Assert.AreEqual(equipmentResponse.requiredLv, equipment.Data.RequiredCharacterLevel);
            Assert.AreEqual(equipmentResponse.rarityId, equipment.Data.Rarity.ID);

            var frameCount = 0;
            while (equipment.Data.Prefab == null || equipment.Passives.Length == 0)
            {
                frameCount++;
                if (frameCount >= 100)
                {
                    Assert.Fail("Cannot load prefab and skill");
                    break;
                    yield break;
                }

                yield return null;
            }

            Assert.AreEqual(equipmentResponse.equipmentIdForeign, equipment.Data.Prefab.ID);
            Assert.AreEqual(equipmentResponse.passiveSkills[0], equipment.Passives[0].Id);
        }
    }
}