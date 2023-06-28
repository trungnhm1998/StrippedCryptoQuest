using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.Character.Movement
{
    [TestFixture]
    public class ConstantVelocityInSingleDirectionStrategyTests
    {
        private IPlayerVelocityStrategy _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ConstantVelocityInSingleDirectionStrategy();
        }

        [TestCase(4, -1, 0, -4, 0)]
        [TestCase(4, 0, 0, 0, 0)]
        [TestCase(5, 0, 1, 0, 5)]
        [TestCase(5, 1, 1, 0, 5)]
        [TestCase(5, 0.6f, 0.2f, 5, 0)]
        [TestCase(5, 0.6f, 0.2f, 5, 0)]
        [TestCase(5, 0.4f, -0.9f, 0, -5)]
        [TestCase(-10, 0, 1, 0, 0)]
        public void CalculateVelocity_WithInput_ShouldReturnOnlyOneDirectionWithFullSpeed(
            float speed,
            float inputX, float inputY,
            float expectedX, float expectedY)
        {
            var velocity = _controller.CalculateVelocity(new Vector2(inputX, inputY), speed);

            var expectedVelocity = new Vector2(expectedX, expectedY);

            Assert.AreEqual(expectedVelocity, velocity);
        }


        [TestCase(4, 1, 0, 1, 0.5f, 4, 0)]
        [TestCase(4, 1, 0, -1, 0, -4, 0)]
        [TestCase(4, 1, 0, 1, 1, 4, 0)]
        [TestCase(4, 1, 0, 0.7f, 0.7f, 4, 0)]
        [TestCase(4, 1, 1, 0.9f, 0.7f, 4, 0)]
        [TestCase(4, 0, -1, -1, 0.7f, -4, 0)]
        [TestCase(4, 1, 0, 0, 1, 0, 4)]
        public void CalculateVelocity_SecondInput_ShouldPrioritizeFirstInput(
            float speed,
            float inputX, float inputY,
            float secondInputX, float secondInputY,
            float expectedX, float expectedY)
        {
            var velocity = _controller.CalculateVelocity(new Vector2(inputX, inputY), speed);

            velocity = _controller.CalculateVelocity(new Vector2(secondInputX, secondInputY), speed);

            var expectedVelocity = new Vector2(expectedX, expectedY);

            Assert.AreEqual(expectedVelocity, velocity);
        }
    }
}