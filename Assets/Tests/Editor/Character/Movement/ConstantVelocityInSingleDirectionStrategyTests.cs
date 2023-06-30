using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor.Character.Movement
{
    [TestFixture]
    public class ConstantVelocityInSingleDirectionStrategyTests
    {
        private class MockBehaviour : CharacterBehaviour
        {
            public override bool IsWalking
            {
                set { }
            }

            public override void SetFacingDirection(Vector2 velocity) { }
        }

        private IPlayerVelocityStrategy _controller;

        [SetUp]
        public void Setup()
        {
            var mockBehaviour = new GameObject().AddComponent<MockBehaviour>();
            _controller = new ConstantVelocityInSingleDirectionStrategy(mockBehaviour);
        }

        [TestCase(4, -1, 0, -4, 0)]
        [TestCase(4, 0, 0, 0, 0)]
        [TestCase(5, 0, 1, 0, 5)]
        [TestCase(5, 1, 1, 5, 0)]
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

        [TestCase(4, 0, 1, 0.7f, 0.7f, 4, 0)]
        [TestCase(4, 0, -1, -1, 1, -4, 0)]
        [TestCase(4, 0, -0.8f, 1, -1, 4, 0)]
        [TestCase(4, 0.3f, -0.8f, -0.5f, -0.7f, 0, -4)]
        public void CalculateVelocity_SecondInput_ShouldPrioritizeHorizontal(
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

        [TestCase(4, 1, 0, 1, 0.5f, 4, 0)]
        [TestCase(4, 1, 0, -1, 0, -4, 0)]
        [TestCase(4, 1, 0, 1, 1, 4, 0)]
        [TestCase(4, 1, 0, 0.7f, 0.7f, 4, 0)]
        [TestCase(4, 0, -0.8f, -0.8f, -0.2f, -4, 0)]
        [TestCase(4, 1, 0, 0.7f, 0.7f, 4, 0)]
        [TestCase(4, 1, 1, 0.9f, 0.7f, 4, 0)]
        [TestCase(4, 0, -1, -1, 0.7f, -4, 0)]
        [TestCase(4, 1, 0, 0, 1, 0, 4)]
        [TestCase(4, 0, -0.8f, -0.8f, -0.2f, -4, 0)]
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

        [TestCase(4, 1, 0, 0.00262168166f, -0.00786504522f, 0, -4)]
        public void CalculateVelocity_ThirdInput_ShouldResetLastInput(
            float speed,
            float inputX, float inputY,
            float thirdInputX, float thirdInputY,
            float expectedX, float expectedY)
        {
            // will cached _lastInputVector to be (1, 0)
            var velocity = _controller.CalculateVelocity(new Vector2(inputX, inputY), speed);

            velocity = _controller.CalculateVelocity(Vector2.zero, speed);

            Assert.AreEqual(Vector2.zero, velocity);

            velocity = _controller.CalculateVelocity(new Vector2(thirdInputX, thirdInputY), speed);

            var expectedVelocity = new Vector2(expectedX, expectedY);

            Assert.AreEqual(expectedVelocity, velocity);
        }

        [TestCase(4,
            0.9f, 0,
            0.8f, 0.3f,
            0.2f, 1f,
            0f, 4f)]
        [TestCase(4,
            -0.9f, 0,
            -0.7f, -0.3f,
            -0.2f, -0.8f,
            0f, -4f)]
        [TestCase(4,
            -0.9f, 0,
            -0.7f, -0.3f,
            1, 1f,
            4f, 0)]
        public void CalculateVelocity_ThirdInput_ShouldOverrideSecond(
            float speed,
            float inputX, float inputY,
            float secondInputX, float secondInputY,
            float thirdInputX, float thirdInputY,
            float expectedX, float expectedY)
        {
            // will cached _lastInputVector to be (-1, 0)
            _controller.CalculateVelocity(new Vector2(inputX, inputY), speed);

            // should also cache _lastInputVector to be (-1, 0)
            _controller.CalculateVelocity(new Vector2(secondInputX, secondInputY), speed);

            // should override _lastInputVector to be (0, -1)
            var velocity = _controller.CalculateVelocity(new Vector2(thirdInputX, thirdInputY), speed);

            var expectedVelocity = new Vector2(expectedX, expectedY);

            Assert.AreEqual(expectedVelocity, velocity);
        }
    }
}