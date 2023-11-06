using CryptoQuest.Tavern.UI;
using CryptoQuest.Tavern.UI.CharacterReplacement;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Tavern
{
    public class TavernController : MonoBehaviour
    {
        public UnityAction ExitTavernEvent;

        [field: Header("Managers")]
        [field: SerializeField] public TavernInputManager TavernInputManager { get; private set; }
        [field: SerializeField] public TavernDialogsManager DialogsManager { get; private set; }

        [field: Header("Overview State")]
        [field: SerializeField] public UIOverview TavernUiOverview { get; private set; }

        [field: Header("Character Replacement State")]
        [field: SerializeField] public UICharacterReplacement UICharacterReplacement { get; private set; }
        [field: SerializeField] public UICharacterList UIGameList { get; private set; }
        [field: SerializeField] public UICharacterList UIWalletList { get; private set; }

        [field: Header("Party Organization State")]
        [field: SerializeField] public UIPartyOrganization UIPartyOrganization { get; private set; }
        [field: SerializeField] public UICharacterList UIParty { get; private set; }
        [field: SerializeField] public UICharacterList UINonParty { get; private set; }
    }
}