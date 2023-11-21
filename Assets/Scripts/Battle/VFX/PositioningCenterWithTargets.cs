using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public interface IPositionCalculator
    {
        Vector3 CalculatePosition(List<Transform> targets);
    }

    public class PositioningCenterWithTargets : MonoBehaviour, IPositionCalculator
    {
        public Vector3 CalculatePosition(List<Transform> targets)
        {
            var sum = Vector3.zero;
            foreach (var transform in targets)
            {
                sum += transform.position;
            }

            if (targets.Count <= 0) return sum;
            return sum / targets.Count;
        }
    }
}