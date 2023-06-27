using CryptoQuest.Character;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.ScriptableObjects;
using NUnit.Framework;

namespace Tests.EditMode.Core
{
    [TestFixture]
    public class PlayerStateSoSmokeTests
    {
        [Test]
        public void PlayerStateSo_Created_WithSouthDirectionByDefault()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:CharacterStateSO");

            var haveOnlyOneInstance = 1;
            Assert.AreEqual(haveOnlyOneInstance, guids.Length);

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            var playerStateSO = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStateSO>(path);

            Assert.AreEqual(CharacterBehaviour.EFacingDirection.South, playerStateSO.FacingDirection);
        }
    }
}