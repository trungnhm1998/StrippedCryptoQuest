using UnityEngine;
using CryptoQuest.Battle.Components;

namespace CryptoQuest.Character.Hero.AvatarProvider
{
    public class HeroBattleAvatarProvider : HeroAvatarProvider
    {
        protected override void AvatarLoaded(HeroBehaviour hero, Sprite avatar)
        {
            hero.BattleAvatar = avatar;
        }
    }
}
