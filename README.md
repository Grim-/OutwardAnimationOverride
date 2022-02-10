# OutwardAnimationOverride
A simple example of how to use Animator Override Controllers to change animations in Outward.

https://imgur.com/3TV3t5w

## Still in Testing
I'd like to preface this by saying, this is still early days, while you can successfully change an animation using this, I haven't tested it fully yet and there is every chance that Outwards Weapon Animations use a similar system for the different Weapon Animation Stances.

So equipping a weapon may full well override your own AnimatorOverrideController but with more testing and understanding of the animation system I dont believe it's a deal breaker - also if the Animation Events for Attacking, Drinking, Using etc are tied to directly to the clip itself then these events wont fire when you override the clip, that does not mean this is useless however as we can configure clips in Unity beforehand to call these events ourselves, with the possibility of writing a script that does this automatically.


This guide assumes you have followed the guides on how to set up your initial Outward Mod project 
https://outward.fandom.com/wiki/Installing_Mods 
and have also installed Sinai's SideLoader https://sinai-dev.github.io/OSLDocs/#/ 
these are both required.

 
 ## Guide 
 
In order to import an AnimationClip correctly into Outward via SideLoader you first need to configure and set up all the animations in a Unity Project, for this example I used the recommended 2018.4.8f1.

Create a new Unity Project with Editor Version 2018.4.8f1, import the animations you want to use into Unity, you can either drag and drop the files into the Asset folder or use "Assets/Import New Asset" from the menu bar.

![image](https://user-images.githubusercontent.com/3288858/152446579-21b2c8e4-40b7-4a03-b7bf-f9b2f3723bc4.png)

Once you have imported the AnimationClip files in (these are also stored inside FBX files so check there if you can't find them) you need to click them in your assets folder, this will bring up the import settings for this file in the inspector, there are four tabs - "Model", "Rig", "Animation" and "Materials" we need the second tab "Rig" on this tab you will see a "Animation Type" dropdown, this must be set to "Humanoid" otherwise the character model will sink into the floor, this is because the character animator component is set to work with Humanoid animations only.

![image](https://user-images.githubusercontent.com/3288858/152446718-5c4d79d2-ec55-4eb4-a197-5e479427a4d1.png)


Once the animation type is set to Humanoid, we now need to create an AssetBundle of all the animations. 


Click "Window > AssetBundle Browser" to open the AssetBundle browser, if you don't have this option under the "Window" menu bar option, you need to install the AssetBundles package from the Unity Package Manager under "Window > Package Manager" and install the AssetBundle Browser package. 

![image](https://user-images.githubusercontent.com/3288858/152446786-bc4effb0-2130-44be-a847-6c6ff8c4c360.png)


Once the window is open you can right click in the grey box to the left hand side of the panel and create a new AssetBundle with "Add new Bundle" **give this bundle a name you can remember as you will be refering to it later. **

![image](https://user-images.githubusercontent.com/3288858/152446907-2a734843-d19c-403c-b469-b6004dad0d16.png)


Once the bundle is created drag all your animations to the right hand panel to add them to the AssetBundle - once again make note of what you name each animation in this bundle as this is how you will refer to it in code later - then once all your animations are imported click the "Build" tab of the AssetBundle browser, change the Output path to whatever you desire now press the "Build" button.

![image](https://user-images.githubusercontent.com/3288858/152447347-24378fa0-e743-42a5-90c9-8243a36950c0.png)
![image](https://user-images.githubusercontent.com/3288858/152447362-dfb97ecd-ad38-4a3c-84bc-ad732e2ed5e9.png)


Once built navigate to the Output Path folder you set, inside here you will find the bundle you created with the same file name as the AssetBundle you created, this is the only file you need you can ignore the .manifest file. 

![image](https://user-images.githubusercontent.com/3288858/152447079-8dc00268-c0de-4eb6-a3fe-d671905ff24a.png)


In this example I changed the HumanIdle animation to Capoeira dancing, so my AssetBundle was named Capoeira and the Animation **inside** that bundle is also called Capoeria.




Now you need to copy this asset bundle into your Mod folder *inside your r2modman profile folder* 


Here I created a folder called "AnimatorOverride" inside my BepInEx/plugins folder and within that I created a folder called SideLoader as per the SideLoader instructions then finally inside the SideLoader folder create another folder called AssetBundles and place your AssetBundle file inside this folder. 

![image](https://user-images.githubusercontent.com/3288858/152447249-b4d3778b-abae-4a50-a30c-dc9759071462.png)


Example : "TestProfile/BepInEx/plugins/YourModFolder/SideLoader/AssetBundles/"


Once you have done all that, you should be ready to actually override some animations, this requires creating a new AnimatorOverride Controller and overriding the animations you need by name, I have included a list of each animation name.

[Animations List](https://github.com/Grim-/OutwardAnimationOverride/files/7998984/message.txt)


## Example

 Below is an example of how the CustomAnimatorOverride's and the AnimatorControllerManager can be used, to use it in your mod for now you must include the OutwardAnimatorOverrideTest.dll in the Release folder (https://github.com/Grim-/OutwardAnimationOverride/tree/main/OutwardModTemplate-main/Release) as part of your project you can then use it as below - this will be replaced and integrated into https://github.com/sinai-dev/Outward-SideLoader later 

```C#
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Text;
using UnityEngine;
using SideLoader;
using System.Reflection;
using OutwardEmoAnim;

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


        //Needed to tell Controller it needs to check the conditions again

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
```

