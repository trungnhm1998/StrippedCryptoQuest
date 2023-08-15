using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Option
{
    /// <summary>
    /// I want a slider that when holding left or right, it will move the slider by 1 step for t seconds then it will move for max 10 steps.
    /// </summary>
    public class UISlider : Slider
    {
        private const float HOLD_DURATION = 0.16f;
        [SerializeField] private Step[] _steps =
        {
            new()
            {
                Threshold = 0.5f,
                StepValue = 1,
            },
            new()
            {
                Threshold = 1.4f,
                StepValue = 5,
            },
            new()
            {
                Threshold = 1.8f,
                StepValue = 10,
            },
        };

        private int _currentStepIndex;

        [Serializable]
        private struct Step
        {
            public float Threshold;
            public float StepValue;
        }

        private enum Axis
        {
            Horizontal = 0,
            Vertical = 1
        }

        private Axis CurrentAxis => direction is Direction.LeftToRight or Direction.RightToLeft
            ? Axis.Horizontal
            : Axis.Vertical;

        private bool IsReversed => direction is Direction.RightToLeft or Direction.TopToBottom;

        private float _lastTime;
        private float _holdTime;

        public override void OnMove(AxisEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
            {
                base.OnMove(eventData);
                return;
            }

            var timeSinceLastCalled = Time.time - _lastTime;
            var isHolding = IsHolding(timeSinceLastCalled);

            _currentStepIndex = IncrementStepIfCrossCurrentThreshold(isHolding, timeSinceLastCalled);

            var currentStep = _steps[_currentStepIndex];

            var stepSize = currentStep.StepValue;

            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (CurrentAxis == Axis.Horizontal && FindSelectableOnLeft() == null)
                    {
                        Set(IsReversed ? value + stepSize : value - stepSize);
                        _lastTime = Time.time;
                    }
                    else
                        base.OnMove(eventData);

                    break;
                case MoveDirection.Right:
                    if (CurrentAxis == Axis.Horizontal && FindSelectableOnLeft() == null)
                    {
                        Set(IsReversed ? value - stepSize : value + stepSize);
                        _lastTime = Time.time;
                    }
                    else
                        base.OnMove(eventData);

                    break;
                default:
                    base.OnMove(eventData);
                    break;
            }
        }

        /// <summary>
        /// if last time this method were called is less than <see cref="HOLD_DURATION"/> then it will return true
        /// could be wrong but I think this is the best way to do it
        /// </summary>
        /// <param name="timeSinceLastCalled"></param>
        /// <returns></returns>
        private bool IsHolding(float timeSinceLastCalled)
        {
            if (!(timeSinceLastCalled >= HOLD_DURATION)) return true;
            ResetToDefaultStep();
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="isHolding"></param>
        /// <param name="timeSinceLastCalled"></param>
        /// <returns></returns>
        private int IncrementStepIfCrossCurrentThreshold(bool isHolding, float timeSinceLastCalled)
        {
            if (!isHolding) return _currentStepIndex;

            _holdTime += timeSinceLastCalled;
            if (!(_holdTime >= _steps[_currentStepIndex].Threshold)) return _currentStepIndex;

            return _currentStepIndex >= _steps.Length - 1 ? _steps.Length - 1 : _currentStepIndex + 1;
        }

        /// <summary>
        /// The hold duration will be reset, step will now be default to +-1
        /// </summary>
        private void ResetToDefaultStep()
        {
            _holdTime = 0;
            _currentStepIndex = 0;
        }

        /* I don't want to remove debug code
        private void OnGUI()
        {
            // draw label for hold time and last time
            GUI.Label(new Rect(10, 10, 200, 20), $"Hold time: {_holdTime.ToString()}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Last time: {_lastTime.ToString()}");
            // label for current step
            GUI.Label(new Rect(10, 50, 200, 20), $"Current step: {_currentStepIndex}");
            // label for thresh hold and step value
            GUI.Label(new Rect(10, 70, 200, 20), $"Threshold: {_steps[_currentStepIndex].Threshold.ToString()}");
            GUI.Label(new Rect(10, 90, 200, 20), $"Step value: {_steps[_currentStepIndex].StepValue}");
        }
        */
    }
}