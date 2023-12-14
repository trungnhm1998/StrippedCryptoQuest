using CryptoQuest.Audio.AudioData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Audio
{
    public class InputSfxController : MonoBehaviour
    {
        [SerializeField] private AudioCueEventChannelSO _playSfxEventSO;

        private ChainValidator _validator;

        private void Awake()
        {
            _validator = new IsSelectingObject();
            _validator.SetNext(new IsButtonInteractable());
        }

        /// <summary>
        /// Play sfx when input and there're something is selected (button, selectable,...) and is active 
        /// </summary>
        /// <param name="cue"></param>
        public void PlayOnInput(AudioCueSO cue)
        {
            if (!_validator.IsValid()) return;
            _playSfxEventSO.PlayAudio(cue);
        }
    }
    
    public abstract class ChainValidator
    {
        protected ChainValidator _nextValidator;

        /// <summary>
        /// This validator valid when there no next or the next also valid
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
            => _nextValidator == null || _nextValidator.IsValid();

        public virtual void SetNext(ChainValidator validator)
        {
            _nextValidator = validator;
        }
    }

    public class IsSelectingObject : ChainValidator
    {
        public override bool IsValid()
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;
            return (selectedObject != null && selectedObject.activeInHierarchy)
                && base.IsValid();
        }
    }

    public class IsButtonInteractable : ChainValidator
    {
        public override bool IsValid()
        {
            var selectedObject = EventSystem.current.currentSelectedGameObject;

            var button = selectedObject.GetComponent<Button>();
            return (button != null && button.interactable)
                && base.IsValid();
        }
    }
}
