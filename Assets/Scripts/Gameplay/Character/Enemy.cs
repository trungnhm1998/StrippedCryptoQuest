using System;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public class Enemy : CharacterInformation<EnemyData, Enemy>
    {
        /// <summary>
        /// To separate with other same Enemy in a group
        /// their name will be post fix with A, B, C, D 
        /// </summary>
        /// <param name="postFix"></param>
        public void SetDisplayName(string postFix)
        {
            // var name = $"{Data.Name.GetLocalizedString()}{postFix}";
        }
    }
}