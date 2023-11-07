using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassStateController : MonoBehaviour
    {
        [field: SerializeField] public ChangeClassDialogController DialogController { get; private set; }
        [field: SerializeField] public ChangeClassInputManager Input { get; private set; }
        [field: SerializeField] public ChangeClassPresenter Presenter { get; private set; }
        [field: SerializeField] public ChangeClassManager Manager { get; private set; }
        [field: SerializeField] public Button DefaultButton { get; private set; }
        public UnityAction ExitStateEvent;
        public UnityAction<bool> EnterSelectMaterialStateEvent;
    }
}
