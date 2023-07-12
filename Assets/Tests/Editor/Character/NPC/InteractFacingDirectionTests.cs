using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Character.NPC
{
    [TestFixture]
    public class InteractFacingDirectionTest
    {
        [TestCase(1, 0, CharacterBehaviour.EFacingDirection.East)]
        [TestCase(0, 1, CharacterBehaviour.EFacingDirection.North)]
        [TestCase(-1, 0, CharacterBehaviour.EFacingDirection.West)]
        [TestCase(0, -1, CharacterBehaviour.EFacingDirection.South)]
        [TestCase(-.9f, 1, CharacterBehaviour.EFacingDirection.North)]
        [TestCase(.9f, 1, CharacterBehaviour.EFacingDirection.North)]
        [TestCase(1, -.9f, CharacterBehaviour.EFacingDirection.East)]
        [TestCase(-.96f, -.95f, CharacterBehaviour.EFacingDirection.West)]
        [TestCase(-.96f, -.99f, CharacterBehaviour.EFacingDirection.South)]
        [TestCase(-.99f, -1, CharacterBehaviour.EFacingDirection.South)]
        [TestCase(13.82344f, 3.244219f, CharacterBehaviour.EFacingDirection.East)]
        public void OnTriggerEnter_PlayerPosition_ShouldFaceCorrect(float xPosition, float yPosition, CharacterBehaviour.EFacingDirection expectedResult)
        {
            // arrange
            IFacingStrategy facingLogic = new NpcFacingStrategy();
            Vector2 playerPosition = new Vector2(xPosition, yPosition);
            Vector2 characterPosition = Vector2.zero;

            // act
            var facingDirection = facingLogic.Execute(characterPosition, playerPosition);

            // assert
            Assert.AreEqual(expectedResult, facingDirection);
        }
    }
}