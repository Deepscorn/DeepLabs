// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using System.Collections;
using Assets.Sources.Util.Log;

namespace Assets.Sources.Scripts.Basic
{
    // usecase: screen have one keyframe animation.
    // Inheritance tips: More animations with one keyframe may be launched/stopped in overriden SetOpen()
    // But do not forget to include them in overriden IsAnimating() to be sure
    // your animations wount be in transition when screen will be deactivated
    public class BlendAnimatorBasedScreen : AbstractScreen
    {
        public string AnimatorOpenBoolParamName = "Open";
        public string AnimatorStateOpenName = "Open";
        public string AnimatorStateClosedName = "Closed";

        private int animatorOpenBoolParamNameHash;
        private Animator animator;

        private Animator Animator
        {
            get
            {
                if (animator == null)
                {
                    animator = GetComponent<Animator>();
                }
                return animator;
            }
        }

        private int AnimatorOpenBoolParamNameHash
        {
            get
            {
                if (animatorOpenBoolParamNameHash == 0)
                {
                    animatorOpenBoolParamNameHash = Animator.StringToHash(AnimatorOpenBoolParamName);
                }
                return animatorOpenBoolParamNameHash;
            }
        }

        // set whether screen must start open / close transition
        public override void SetOpen(bool open)
        {
            Animator.SetBool(AnimatorOpenBoolParamNameHash, open);
        }

        public override IEnumerator WaitForClosing()
        {
            var closedStateReached = false;
            var wantToClose = true;
            while (!closedStateReached && wantToClose)
            {
                if (!IsAnimating())
                {
                    closedStateReached = Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorStateClosedName);
                }
                wantToClose = !IsOpeningOrOpened();

                yield return new WaitForEndOfFrame();
            }
        }

        public override bool IsOpeningOrOpened()
        {
            return Animator.GetBool(AnimatorOpenBoolParamNameHash);
        }

        // any additional animation on which we must wait before diactivating screen
        // must be specified in subclasses
        protected virtual bool IsAnimating()
        {
            return Animator.IsInTransition(0);
        }
    }
}