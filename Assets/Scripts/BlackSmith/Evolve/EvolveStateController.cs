using CryptoQuest.BlackSmith.Evolve.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EvolveStateController : MonoBehaviour
    {
        public UnityAction ExitConfirmPhaseEvent;
        [field: SerializeField] public BlackSmithInputManager Input { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogsPresenter { get; private set; }
        [field: SerializeField] public EvolvePresenter EvolvePanel { get; private set; }
        [field: SerializeField] public UIEvolveEquipmentList EvolveEquipmentList { get; private set; }
        [field: SerializeField] public UIConfirmPanel ConfirmPanel { get; private set; }
        [field: SerializeField] public UIBlackSmith UIBlackSmith { get; private set; }
    }
}