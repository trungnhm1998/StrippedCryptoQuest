using CryptoQuest.BlackSmith.Evolve.UI;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EvolveStateController : MonoBehaviour
    {
        [field: SerializeField] public BlackSmithInputManager Input { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogsPresenter { get; private set; }
        [field: SerializeField] public EvolvePresenter EvolvePanel { get; private set; }
        [field: SerializeField] public UIEvolveEquipmentList EvolveEquipmentList { get; private set; }
        [field: SerializeField] public UIConfirmPanel ConfirmPanel { get; private set; }
    }
}