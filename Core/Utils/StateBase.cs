// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util
{
    public class StateBase : StateMachineBehaviour
    {
        private bool finished;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            finished = false;
        }

        public virtual void OnAnimatorFinish(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // stateInfo.normalizedTime goes above 1 even when animation, attached to state is not looping
            if (!finished && stateInfo.normalizedTime > 1F)
            {
                finished = true;
                OnAnimatorFinish(animator, stateInfo, layerIndex);
            }
        }
    }
}
