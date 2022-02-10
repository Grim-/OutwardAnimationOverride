using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Text;
using UnityEngine;
using SideLoader;
using System.Reflection;


//It seems sign up is still free on https://www.mixamo.com/#/ 

namespace OutwardAOCTest
{
    [BepInDependency(SL.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class OutwardAnimatorOverrideTest : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "emo.aoctest";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "Animation Override Controller Test";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.0.0";

        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource Log;

        public static AnimatorControllerManager controllerManager;

        private bool SetUpRun = false;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;

            SL.OnSceneLoaded += SetUpOverrides;
        }

        private void SetUpOverrides()
        {
            if (!SetUpRun)
            {
                //might not need
                SetUpRun = true;
            }


            //Create the Animator Controller Manager passing in the Animator
            controllerManager = new AnimatorControllerManager(GetPlayerCharacter().Animator);



            //Create a Custom Animator Override with name Ninja
            CustomAnimatorOverride ninjaOverride = controllerManager.CreateAnimatorOverrideController("Ninja");
            //Replace the Crouch Idle Animation with Ninja Idle Animation from SideLoader 
            ninjaOverride.AddConditionalOverride(ninjaOverride.GetClip(AnimationsHelper.Crouch_Idle), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Ninja Idle"),
            () =>
            {
                //check Character has Swift Foot Skill
                return PlayerCharacterHasSkill(8205130);
            });

            //Same but for walking
            ninjaOverride.AddConditionalOverride(ninjaOverride.GetClip(AnimationsHelper.Crouch_Walk_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Ninja Crouch Walk"),
            () =>
            {
                return PlayerCharacterHasSkill(8205130);
            });

            ninjaOverride.AddConditionalOverride(ninjaOverride.GetClip(AnimationsHelper.Crouch_Sprint_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Ninja Crouch Sprint"),
            () =>
            {
                return PlayerCharacterHasSkill(8205130);
            });


            //Naruto Running requires Swift Foot & Feather Dodge
            ninjaOverride.AddConditionalOverride(ninjaOverride.GetClip(AnimationsHelper.Default_Sprint_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Ninja Run"),
            () =>
            {
                return PlayerCharacterHasSkill(8205130) && PlayerCharacterHasSkill(8205120);
            });

            //Set the Ninja Controller active
            controllerManager.SetOverrideAnimatorController("Ninja");



            //Create another called Zombie, which will be set when Z is pressed
            CustomAnimatorOverride zombieOverride = controllerManager.CreateAnimatorOverrideController("Zombie");
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Default_Idle), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Thriller"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Tired_Idle), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Idle"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Default_Walk_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Walking"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Tired_Walk_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Walking"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Default_Sprint_Forward), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Running"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Default_Roll), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Roll"));
            zombieOverride.ReplaceAnimation(zombieOverride.GetClip(AnimationsHelper.Attack_Unarmed), GetAnimationClipFromSideLoadPack("animationbundle", "animationsbundle", "Zombie Attack"));

            //need to patch it after load
            new Harmony(GUID).PatchAll();
        }

        private bool PlayerCharacterHasSkill(int itemID)
        {
            return GetPlayerCharacter().Inventory.SkillKnowledge.GetItemFromItemID(itemID) != null;
        }

        internal void Update()
        {
            //revert to original controller
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                controllerManager.RevertToOriginalController();
            }

            //change to zombie
            if (Input.GetKeyDown(KeyCode.Z))
            {
                controllerManager.SetOverrideAnimatorController("Zombie");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N))
            {
                controllerManager.RevertToPreviousController();
            }
        }

        //niet good
        private Character GetPlayerCharacter()
        {
            return CharacterManager.Instance.Characters.m_values[0];
        }
        public static AnimationClip GetAnimationClipFromSideLoadPack(string sideLoaderPackName, string sideLoaderBundleName, string animationClipName)
        {
            SLPack sideLoaderPack = SL.GetSLPack(sideLoaderPackName);

            if (sideLoaderPack != null)
            {
                if (sideLoaderPack.AssetBundles[sideLoaderBundleName] != null)
                {
                    if (!sideLoaderPack.AssetBundles[sideLoaderBundleName].Contains(animationClipName))
                    {
                        Log.LogMessage($"Animation Clip not found with name : {animationClipName}.");
                        return null;
                    }

                    return sideLoaderPack.AssetBundles[sideLoaderBundleName].LoadAsset<AnimationClip>(animationClipName);                  
                }
                else
                {
                    Log.LogMessage($"SideLoader AssetBundle with Name : {sideLoaderBundleName} not found. ");
                    return null;
                }
            }

            Log.LogMessage($"SideLoader Pack with Name : {sideLoaderPackName} not found. ");
            return null;
        }

        #region Harmony

        [HarmonyPatch(typeof(CharacterSkillKnowledge), nameof(CharacterSkillKnowledge.AddItem))]
        public class CharacterSkillKnowledge_AddItem
        {
            static void Postfix()
            {
                if (controllerManager != null)
                {
                    //since there's no events this will have to do, requires the controller manager to be a singleton, but really there should be only one anyway, will help resolving duplicate override rules
                    controllerManager.TriggerConditionCheck();
                }
            }
        }

        [HarmonyPatch(typeof(CharacterSkillKnowledge), nameof(CharacterSkillKnowledge.RemoveItem))]
        public class CharacterSkillKnowledge_RemoveItem
        {
            static void Postfix()
            {
                if (controllerManager != null)
                {
                    //since there's no events this will have to do, requires the controller manager to be a singleton, but really there should be only one anyway, will help resolving duplicate override rules
                    controllerManager.TriggerConditionCheck();
                }
            }
        }
        #endregion
    }
}


