using CryptoQuest.Tavern.UI;
using CryptoQuest.Tavern.UI.CharacterReplacement;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Tavern
{
    public class TavernController : MonoBehaviour
    {
        public UnityAction ExitTavernEvent;

        [field: SerializeField] public TavernInputManager TavernInputManager { get; private set; }
        [field: SerializeField] public TavernDialogsManager DialogsManager { get; private set; }

        [field: SerializeField] public UIOverview TavernUiOverview { get; private set; }
        [field: SerializeField] public UICharacterReplacement UICharacterReplacement { get; private set; }
        [field: SerializeField] public UICharacterList UIGameList { get; private set; }
        [field: SerializeField] public UICharacterList UIWalletList { get; private set; }
    }
}