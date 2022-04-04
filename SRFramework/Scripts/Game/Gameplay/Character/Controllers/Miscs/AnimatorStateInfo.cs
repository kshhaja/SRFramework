using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeRPG.Event
{
    public class AnimatorStateInfos
    {
        public bool debug;
        public Animator animator;
        public AnimatorStateInfos(Animator animator)
        {
            this.animator = animator;
        }

        public void RegisterListener()
        {

        }

        public void RemoveListener()
        {

        }

        public bool HasTag(string tag)
        {
            return true;
        }

        public bool HasAnimatorLayerUsingTag(string tag, out int layer)
        {
            layer = 0;
            return true;
        }
    }
}