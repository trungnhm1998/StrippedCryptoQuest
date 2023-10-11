using UnityEngine;
using CryptoQuest.Battle.Components;

namespace CryptoQuest.Character.Hero.AvatarProvider
{
    public class HeroBattleAvatarProvider : HeroAvatarProvider
    {
        protected override bool CheckValidAvatar(HeroBehaviour hero)
        {
            if (hero.BattleAvatar != null)
            {
                AvatarLoaded(hero, hero.BattleAvatar);
                return true;
            }
            return false;
        }

        protected override void AvatarLoaded(HeroBehaviour hero, Sprite avatar)
        {
            hero.BattleAvatar = avatar;
        }
    }
}
