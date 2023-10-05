using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EvolveStateController : MonoBehaviour
    {
        [field: SerializeField] public BlackSmithInputManager Input { get; private set; }
        [field: SerializeField] public GameObject EvolvePanel { get; private set; }
    }
}