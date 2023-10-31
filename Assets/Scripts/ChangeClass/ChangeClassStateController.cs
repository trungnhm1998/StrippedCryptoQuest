using UnityEngine.Events;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassStateController : MonoBehaviour
    {
        [field: SerializeField] public ChangeClassInputManager Input { get; private set; }
        public UnityAction ExitStateEvent;
    }
}
