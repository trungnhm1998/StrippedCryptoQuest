using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Character.NPC
{
    [TestFixture]
    public class InteractFacingDirectionTest
    {
        [TestCase(0, 0, 1, 0, CharacterBehaviour.EFacingDirection.East)]
        [TestCase(0, 0, 0, 1, CharacterBehaviour.EFacingDirection.North)]
        [TestCase(0, 0, -1, 0, CharacterBehaviour.EFacingDirection.West)]
        [TestCase(0, 0, 0, -1, CharacterBehaviour.EFacingDirection.South)]

        [TestCase(0, 0, -.9f, 1, CharacterBehaviour.EFacingDirection.North)]
        [TestCase(0, 0, .9f, 1, CharacterBehaviour.EFacingDirection.North)]

        [TestCase(0, 0, 1, -.9f, CharacterBehaviour.EFacingDirection.East)]
        [TestCase(0, 0, -.96f, -.95f, CharacterBehaviour.EFacingDirection.West)]

        [TestCase(0, 0, -.96f, -.99f, CharacterBehaviour.EFacingDirection.South)]
        [TestCase(0, 0, -.99f, -1, CharacterBehaviour.EFacingDirection.South)]
        [TestCase(14.09843f, 2.700001f, 13.82344f, 3.244219f, CharacterBehaviour.EFacingDirection.West)]
        public void OnTriggerEnter_PlayerPosition_ShouldFaceCorrect(
            float myX,
            float myY,
            float playerX,
            float playerY,
            CharacterBehaviour.EFacingDirection expectedResult)
        {
            // arrange
            IFacingStrategy facingLogic = new NpcFacingStrategy();

            // act
            var facingDirection = facingLogic.Execute(new Vector2(myX, myY), new Vector2(playerX, playerY));

            // assert
            Assert.AreEqual(expectedResult, facingDirection);
        }
    }
}