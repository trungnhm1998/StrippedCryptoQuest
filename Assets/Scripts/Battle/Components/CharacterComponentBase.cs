using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public abstract class CharacterComponentBase : MonoBehaviour
    {
        private Character _character;
        protected Character Character => _character ??= base.GetComponent<Character>();

        public void Init()
        {
            OnInit();
        }

        /// <summary>
        /// Called after <see cref="HeroSpec"/> provided
        /// </summary>
        protected virtual void OnInit() { }

        public void Reset() => OnReset();

        /// <summary>
        /// If this character is valid OnReset will be called when the character is destroyed.
        /// </summary>
        protected virtual void OnReset() { }

        public new T GetComponent<T>() => Character.GetComponent<T>();
    }
}