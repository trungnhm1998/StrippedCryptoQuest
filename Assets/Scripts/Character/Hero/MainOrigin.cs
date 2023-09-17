using CryptoQuest.Character.Hero;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    public class MainOrigin : Origin
    {
        [field: SerializeField] public string Name { get; set; }

        public override void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetName(Name);
        }
    }
}