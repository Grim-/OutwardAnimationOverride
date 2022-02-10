using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OutwardEmoAnim
{
    [System.Serializable]
    public class CustomAnimatorOverride
    {
        public AnimatorOverrideController Controller;
        public List<AnimationOverrideCondition> ConditionalOverrides;

        private bool Debug = false;

        public CustomAnimatorOverride(AnimatorOverrideController controller)
        {
            Controller = controller;
            ConditionalOverrides = new List<AnimationOverrideCondition>();
        }

        /// <summary>
        /// Add a Conditional Override, the clip is overriden if ConditionCheck results to true
        /// </summary>
        /// <param name="originalClip"></param>
        /// <param name="newClip"></param>
        /// <param name="ConditionCheck"></param>
        public void AddConditionalOverride(AnimationClip originalClip, AnimationClip newClip, Func<bool> ConditionCheck)
        {
            if (ConditionalOverrides.Find(x => x.OriginalAnimation == originalClip && x.NewAnimation == newClip) == null)
            {
                //no override for this clip combo
                ConditionalOverrides.Add(new AnimationOverrideCondition(originalClip, newClip, ConditionCheck));
            }
        }
        /// <summary>
        /// Get a Clip by name from Override Controller
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public AnimationClip GetClip(string clipName)
        {
            foreach (var item in Controller.animationClips)
            {
                if (item.name == clipName)
                {
                    //OutwardAnimatorOverrideTest.Log.LogMessage("Current Override Found Clip With Name : " + clipName);
                    return item;
                }
            }

            //OutwardAnimatorOverrideTest.Log.LogMessage(clipName + " Clip not found in Current Override Controller");
            return null;
        }
        /// <summary>
        /// Replace OriginalClip with NewClip optional parameter to copy animation events
        /// </summary>
        /// <param name="originalClip"></param>
        /// <param name="newClip"></param>
        /// <param name="copyEvents"></param>
        public void ReplaceAnimation(AnimationClip originalClip, AnimationClip newClip, bool copyEvents = true)
        {
            if (copyEvents)
            {
                CopyAnimationEvents(originalClip, newClip);
            }
            else
            {
                  //if(Debug)  OutwardAOCTest.OutwardAnimatorOverrideTest.Log.LogMessage("Animation has no events to copy");
            }

            Controller.SetClip(originalClip, newClip, true);
        }
        /// <summary>
        /// Checks if the given AnimationClip is currently Overriden
        /// </summary>
        /// <param name="animationClip"></param>
        /// <returns></returns>
        public bool AnimationIsOverriden(AnimationClip animationClip)
        {
            return Controller.GetOverrideClip(animationClip) != null;
        }
        /// <summary>
        /// Copy Animation Events from one clip to another
        /// </summary>
        /// <param name="originalClip"></param>
        /// <param name="newClip"></param>
        private void CopyAnimationEvents(AnimationClip originalClip, AnimationClip newClip)
        {
            //OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("Copying Animation Events from {0} to {1}", originalClip.name, newClip.name));

            if (originalClip == null)
            {
                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("originalClip is null"));
                return;
            }

            if (originalClip.events == null)
            {
                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("original clip events array is null"));
                return;
            }

            if (originalClip.events.Length == 0)
            {
                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("original clip events array is empty"));
                return;
            }

            if (newClip == null)
            {
               //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("new clip is null"));
                return;
            }

            List<AnimationEvent> copyOfOriginalEvents = originalClip.events.ToList();

            for (int i = 0; i < copyOfOriginalEvents.Count; i++)
            {
                if (copyOfOriginalEvents[i] == null)
                {
                   // if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("Animation Event is null skipping"));
                    continue;
                }
                else
                {
                    //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("Copying Event name {0} {1}", copyOfOriginalEvents[i].m_FunctionName, copyOfOriginalEvents[i].m_Time));


                    AnimationEvent newEvent = copyOfOriginalEvents[i];
                    newEvent.m_Time = ScaleAnimationTimeWithClipLength(originalClip.length, copyOfOriginalEvents[i].m_Time, newClip.length);
                    newClip.AddEvent(newEvent);
                }
            }
        }
        /// <summary>
        /// Scales an AnimationEvent time by the newClip duration
        /// </summary>
        /// <param name="originalClipDuration">Original Clip Duration</param>
        /// <param name="originalClipEventTime">Original Clip Animation Event trigger time</param>
        /// <param name="newClipDuration">New Clip Duration</param>
        /// <returns> Animation Event trigger time scaled to New Clip Duration </returns>
        private float ScaleAnimationTimeWithClipLength(float originalClipDuration, float originalClipEventTime, float newClipDuration)
        {
            return newClipDuration * (originalClipEventTime / originalClipDuration);
        }
    }

    [System.Serializable]
    public class AnimationOverrideCondition
    {
        public AnimationClip OriginalAnimation;
        public AnimationClip NewAnimation;

        private Func<bool> Condition;

        public AnimationOverrideCondition(AnimationClip originalAnimation, AnimationClip newAnimation, Func<bool> condition)
        {
            OriginalAnimation = originalAnimation;
            NewAnimation = newAnimation;
            Condition = condition;
        }

        public bool CheckCondition()
        {
            return Condition.Invoke();
        }

        /// <summary>
        /// Returns clip based on Condition true or false (true = NewAnimation, false = OldAnimation)
        /// </summary>
        /// <returns></returns>
        public AnimationClip GetClip()
        {
            if (Condition.Invoke())
            {
                return NewAnimation;
            }

            return OriginalAnimation;
        }
    }
}

