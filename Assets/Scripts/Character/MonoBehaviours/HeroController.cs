using System;
using System.Collections;
using CryptoQuest.Character.Movement;
using CryptoQuest.Utils;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class HeroController : MonoBehaviour, ICharacterController
    {
        [SerializeField] private Animator _animator;

        public IEnumerable CoPlayAnimation(string animationName)
        {
            AnimationClip clip = _animator.FindAnimation(animationName);
            if (clip == null)
            {
                Debug.Log("[HeroController] Animation not found: " + animationName);
                yield break;
            }

            _animator.Play(animationName);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
