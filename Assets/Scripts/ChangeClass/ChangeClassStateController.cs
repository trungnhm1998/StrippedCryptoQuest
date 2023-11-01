using UnityEngine.Events;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassStateController : MonoBehaviour
    {
        [field: SerializeField] public ChangeClassDialogController DialogController { get; private set; }
        [field: SerializeField] public ChangeClassInputManager Input { get; private set; }
        [field: SerializeField] public ChangeClassManager Manager { get; private set; }
        public UnityAction ExitStateEvent;
    }
}
