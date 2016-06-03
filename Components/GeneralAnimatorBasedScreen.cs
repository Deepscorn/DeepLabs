// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections;
using UnityEngine;

namespace Assets.Sources.Scripts.Basic
{
    public class GeneralAnimatorBasedScreen : AbstractScreen
    {
        public string AnimatorOpenBoolParamName = "Open";
        public float SecondsToWaitBeforeClose = 5f;

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
            yield return new WaitForSeconds(SecondsToWaitBeforeClose);
        }

        public override bool IsOpeningOrOpened()
        {
            return Animator.GetBool(AnimatorOpenBoolParamNameHash);
        }
    }

}
