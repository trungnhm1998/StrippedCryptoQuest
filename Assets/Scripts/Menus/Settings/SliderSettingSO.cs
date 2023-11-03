using CryptoQuest.Events;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Settings
{
    public class SliderSettingSO : ScriptableObject
    {
        /// <summary>
        /// Serialized field for show in inspector custom volume
        /// Hide in inspector because we have custom editor UI for this
        /// </summary>
        [SerializeField, HideInInspector] private float _value = 1f;

        public event UnityAction<float> ValueChanged;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                this.CallEventSafely(ValueChanged, _value);
            }
        }
    }
}
