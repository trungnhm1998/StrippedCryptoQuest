using CryptoQuest.Character.MonoBehaviours;
using NUnit.Framework;

namespace Tests.EditMode.CryptoQuest.Character
{
    [TestFixture]
    public class CharacterMovementTests
    {
        [Test]
        public void CalculateVelocity_ShouldReturnCorrect()
        {
            const float speed = 4f;
            ICharacterController2D controller = new TopDownController(speed);

            controller.InputVector = new UnityEngine.Vector2(0, -1f);

            var velocity = controller.CalculateVelocity();

            var expectedVelocity = new UnityEngine.Vector2(0, -speed);

            Assert.AreEqual(expectedVelocity, velocity);
        }
    }
}