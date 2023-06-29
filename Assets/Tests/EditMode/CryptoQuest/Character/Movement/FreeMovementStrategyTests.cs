using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.Character.Movement
{
    [TestFixture]
    public class FreeMovementStrategyTests
    {
        [Test]
        public void CalculateVelocity_ShouldReturnCorrect()
        {
            const float speed = 4f;
            IPlayerVelocityStrategy controller = new FreeMovementStrategy();


            var velocity = controller.CalculateVelocity(new Vector2(0, -1f), speed);

            var expectedVelocity = new Vector2(0, -speed);

            Assert.AreEqual(expectedVelocity, velocity);
        }
    }
}