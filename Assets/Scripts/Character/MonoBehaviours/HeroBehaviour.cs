using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class HeroBehaviour : CharacterBehaviour
    {
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

        public override void SetFacingDirection(Vector2 velocity)
        {
            base.SetFacingDirection(velocity);

            _interactionCollider.transform.localPosition = _facingOffsetDictionary[FacingDirection].Offset;
        }
    }
}