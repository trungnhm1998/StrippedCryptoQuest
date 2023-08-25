using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// We have Eric_Warrior, Eric_Priest, Eric_Wizard with different avatar
    ///
    /// TODO: Use primitive type for faster loading
    /// </summary>
    public class CharacterBase : ScriptableObject
    {
        [field: SerializeField] public CharacterBackgroundInfo BackgroundInfo { get; private set; }
        [field: SerializeField] public CharacterClass Class { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }

        public void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetAvatar(Avatar);
            uiCharacterInfo.SetClass(Class.Name);
            BackgroundInfo.SetupUI(uiCharacterInfo);
        }
    }
}