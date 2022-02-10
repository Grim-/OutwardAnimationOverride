using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace OutwardEmoAnim
{
    public class AnimatorControllerManager
    {
        #region Properties
        public AnimatorOverrideController CurrentOverride
        {
            get
            {
                return _CurrentOverride;
            }
        }

        public Action<string, AnimatorOverrideController> OnOverrideControllerApplied;

        #endregion;

        #region Fields
        private Animator Controller;
        private AnimatorOverrideController _CurrentOverride = null;
        private RuntimeAnimatorController _Original = null;
        private AnimatorOverrideController _Previous = null;
        private bool Debug = false;

        private Dictionary<string, CustomAnimatorOverride> _Animators = new Dictionary<string, CustomAnimatorOverride>();
        #endregion


        public AnimatorControllerManager(Animator controller)
        {
            Controller = controller;
            _Original = controller.runtimeAnimatorController;
        }

        public void SetOverrideAnimatorController(string name)
        {
            AnimatorOverrideController overrideController = GetAnimatorControllerByName(name).Controller;

            if (overrideController != null)
            {
                SetOverrideAnimatorController(overrideController);
                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage(string.Format("Setting Override Controller to {0}", name));
                OnOverrideControllerApplied?.Invoke(name, overrideController);
            }
        }

        private void SetOverrideAnimatorController(AnimatorOverrideController newController)
        {
            if (_CurrentOverride != null)
            {
                _Previous = _CurrentOverride;
            }
          
            _CurrentOverride = newController;
            SetRunTimeAnimatorController(newController);
        }

        private void SetRunTimeAnimatorController(RuntimeAnimatorController runtimeAnimatorController)
        {
            Controller.runtimeAnimatorController = runtimeAnimatorController;
        }

        /// <summary>
        /// Trigger a Condition Check for the Controller
        /// </summary>
        public void TriggerConditionCheck()
        {
           // if(Debug) OutwardAnimatorOverrideTest.Log.LogMessage("Condition Check Triggered");

            foreach (var item in _Animators)
            {

               // if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage($"Checking {item.Key}  Conditions");
                foreach (var _item in item.Value.ConditionalOverrides.ToList())
                {
                    if (_item.CheckCondition())
                    {
                        //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage($"Passed Condition for {_item.OriginalAnimation.name} overriding to {_item.NewAnimation.name}");
                        item.Value.ReplaceAnimation(_item.OriginalAnimation, _item.NewAnimation);
                    }
                    else
                    {
                        //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage($"Failed Condition for {_item.OriginalAnimation.name}");
                        CustomAnimatorOverride customAnimatorOverride = GetAnimatorControllerByName(item.Key);


                        if (customAnimatorOverride != null)
                        {
                            if (customAnimatorOverride.AnimationIsOverriden(_item.OriginalAnimation))
                            {
                                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage($"Reverting {_item.NewAnimation.name} to {_item.OriginalAnimation.name}");
                                //remove it
                                customAnimatorOverride.ReplaceAnimation(_item.OriginalAnimation, null, false);
                            }
                        }
                    }
                }
                
            }
        }
        /// <summary>
        /// Reverts the Animator Controller to the Default Animator Controller
        /// </summary>
        public void RevertToOriginalController()
        {
            _CurrentOverride = null;
            SetRunTimeAnimatorController(_Original);
            //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage("Returning to Original controller");
        }
        public void RevertToPreviousController()
        {
            if (_Previous != null)
            {
                SetRunTimeAnimatorController(_Previous);
            }
        }
        /// <summary>
        /// Creates a new Animator Override Controller with name and stores it in Dict.
        /// </summary>
        /// <param name="name"></param>
        public CustomAnimatorOverride CreateAnimatorOverrideController(string name)
        {
            if (!_Animators.ContainsKey(name))
            {
                //if (Debug) OutwardAnimatorOverrideTest.Log.LogMessage($"Adding New Animator Override Controller with name : {name}");
                CustomAnimatorOverride customAnimatorOverride = new CustomAnimatorOverride(new AnimatorOverrideController(_Original));
                _Animators.Add(name, customAnimatorOverride);

                return customAnimatorOverride;
            }

            return null;
        }
        /// <summary>
        /// Returns an Animator Override Controller by Name from Dict.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CustomAnimatorOverride GetAnimatorControllerByName(string name)
        {
            if (_Animators.ContainsKey(name))
            {
                return _Animators[name];
            }
            else
            {
                //doesnt exist
                return null;
            }
        }

        private float ScaleAnimationTimeWithClipLength(float originalClipDuration, float originalClipEventTime, float newClipDuration)
        {
            return newClipDuration * (originalClipEventTime / originalClipDuration);
        }
    }
}

