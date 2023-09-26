using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    [RequireComponent(typeof(HeroBehaviour))]
    public class Equipments : CharacterComponentBase
    {
        private HeroBehaviour _heroBehaviour;

        protected override void Awake()
        {
            base.Awake();
            _heroBehaviour = GetComponent<HeroBehaviour>();
        }

        public override void Init() { }
    }
}