using UnityEngine;

namespace CryptoQuest.Utils
{
    public static class AnimatorExtensions
    {
        public static AnimationClip FindAnimation(this Animator animator, string name)
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
    }
}
