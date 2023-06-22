using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CryptoQuest;

namespace Tests.EditMode
{
    [TestFixture]
    public class PlayerStateSoSmokeTests
    {
        [Test]
        public void PlayerStateSo_CreatedCorrectly()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:CharacterStateSO");

            foreach (var guid in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var playerStateSO = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStateSO>(path);
                Assert.IsNotEmpty(playerStateSO.facingDirection.ToString());
            }
        }
    }
}
