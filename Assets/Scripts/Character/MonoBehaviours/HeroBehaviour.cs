using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class HeroBehaviour : CharacterBehaviour
    {
        public event Action Step;

        [Serializable]
        public struct FacingOffset
        {
            public EFacingDirection Direction;
            public Vector2 Offset;
        }

        [SerializeField] private BoxCollider2D _interactionCollider;
        [SerializeField] private FacingOffset[] _facingOffsets = new FacingOffset[4];

        private readonly Dictionary<EFacingDirection, FacingOffset> _facingOffsetDictionary =
            new Dictionary<EFacingDirection, FacingOffset>();

        private void Awake()
        {
            foreach (var facingOffset in _facingOffsets)
            {
                _facingOffsetDictionary.Add(facingOffset.Direction, facingOffset);
            }
        }

        private void OnEnable()
        {
            SetFacingDirection(_facingDirection);
        }

        public IEnumerator ActivateOcarina()
        {
            SetFacingDirection(EFacingDirection.South);
            _animatorComponent.Play("Hero_Ocarina");
            float length = FindAnimation(_animatorComponent, "Hero_Ocarina").length;
            yield return new WaitForSeconds(length);
        }

        public AnimationClip FindAnimation(Animator animator, string name)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }

            return null;
        }

        public override void SetFacingDirection(Vector2 velocity)
        {
            base.SetFacingDirection(velocity);

            _interactionCollider.transform.localPosition = _facingOffsetDictionary[_facingDirection].Offset;
        }

        public void OnStep()
        {
            Step?.Invoke();
        }
    }
}