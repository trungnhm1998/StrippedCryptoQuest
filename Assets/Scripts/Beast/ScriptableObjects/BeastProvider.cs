using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Beast.ScriptableObjects
{
    public class BeastProvider : ScriptableObject
    {
        public UnityAction<IBeast> EquippingBeastChanged;
        [SerializeReference, SubclassSelector] private IBeast _beast = NullBeast.Instance;

        public IBeast EquippingBeast
        {
            get => _beast;
            set
            {
                _beast = value;
                EquippingBeastChanged?.Invoke(_beast);
            }
        }

        private void OnValidate()
        {
            EquippingBeast = _beast;
        }
    }
}