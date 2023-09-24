using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    [RequireComponent(typeof(Hero))]
    public class Equipments : CharacterComponentBase
    {
        private Hero _hero;

        protected override void Awake()
        {
            base.Awake();
            _hero = GetComponent<Hero>();
        }

        public override void Init() { }
    }
}