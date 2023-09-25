using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.Character.Hero
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