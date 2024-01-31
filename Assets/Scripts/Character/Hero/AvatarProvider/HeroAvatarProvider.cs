using System.Collections;
using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Character.Hero.AvatarProvider
{
    public interface IHeroAvatarProvider
    {
        IEnumerator LoadAvatarAsync(HeroBehaviour hero);
    }

    public class HeroAvatarProvider : MonoBehaviour, IHeroAvatarProvider
    {
        [SerializeField] private HeroAvatarDatabase _avatarDatabase;

        /// <summary>
        /// Load asset of character base in its information
        /// <para> 
        /// Whenever character's information changed such as change class, element,
        /// you should use this to reload asset
        /// </para>
        /// There's an event raised the load is succecced or failed
        /// </summary>
        public IEnumerator LoadAvatarAsync(HeroBehaviour hero)
        {
            if (!hero.IsValid())
            {
                Debug.LogWarning($"Hero {hero} is not valid!");
                yield break;
            }

            var id = $"{hero.DetailsInfo.Id}-{hero.Class.Id}";

            yield return _avatarDatabase.LoadDataById(id);
            AvatarLoaded(hero, _avatarDatabase.GetDataById(id));
        }

        protected virtual bool CheckValidAvatar(HeroBehaviour hero)
        {
            if (hero.Avatar != null)
            {
                AvatarLoaded(hero, hero.Avatar);
                return true;
            }

            return false;
        }

        protected virtual void AvatarLoaded(HeroBehaviour hero, Sprite avatar)
        {
            hero.Avatar = avatar;
        }
    }
}