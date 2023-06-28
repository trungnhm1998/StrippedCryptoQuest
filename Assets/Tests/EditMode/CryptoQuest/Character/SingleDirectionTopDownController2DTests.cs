using CryptoQuest.Character;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.Character
{
    [TestFixture]
    public class SingleDirectionTopDownController2DTests
    {
        private ICharacterController2D _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new SingleDirectionTopDownController2D();
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
            _controller.Speed = speed;
            _controller.InputVector = new Vector2(inputX, inputY);

            var velocity = _controller.CalculateVelocity();

            var expectedVelocity = new Vector2(expectedX, expectedY);

            Assert.AreEqual(expectedVelocity, velocity);
        }

        [TestCase(4, 1, 0, -1, 0, -4, 0)]
        public void CalculateVelocity_SecondInputVectorWithDifferentDirection_ShouldChangeDirection(
            float speed,
            float inputX, float inputY,
            float secondInputX, float secondInputY,
            float expectedX, float expectedY)
        {
            _controller.Speed = speed;
            _controller.InputVector = new Vector2(inputX, inputY);
            
            _controller.CalculateVelocity();
            
            _controller.InputVector = new Vector2(secondInputX, secondInputY);
            
            var velocity = _controller.CalculateVelocity();
            
            var expectedVelocity = new Vector2(expectedX, expectedY);
            
            Assert.AreEqual(expectedVelocity, velocity);
        }
    }
}