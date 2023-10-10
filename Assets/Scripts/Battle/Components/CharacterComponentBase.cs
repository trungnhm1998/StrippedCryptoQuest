using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public abstract class CharacterComponentBase : MonoBehaviour
    {
        private Character _character;
        public Character Character => _character;

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
        }

        /// <summary>
        /// Called after <see cref="HeroSpec"/> provided
        /// </summary>
        public abstract void Init();
    }
}