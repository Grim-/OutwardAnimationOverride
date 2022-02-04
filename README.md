# OutwardAnimationOverride
A simple example of how to use Animator Override Controllers to change animations in Outward.

<blockquote class="imgur-embed-pub" lang="en" data-id="3TV3t5w" data-context="false" ><a href="//imgur.com/3TV3t5w"></a></blockquote><script async src="//s.imgur.com/min/embed.js" charset="utf-8"></script>


## Still in Testing
I'd like to preface this by saying, this is still early days, while you can successfully change an animation using this, I haven't tested it fully yet and there is every chance that Outwards Weapon Animations use a similar system for the different Weapon Animation Stances.

So equipping a weapon may full well override your own AnimatorOverrideController but with more testing and understanding of the animation system I dont believe it's a deal breaker - also if the Animation Events for Attacking, Drinking, Using etc are tied to directly to the clip itself then these events wont fire when you override the clip, that does not mean this is useless however as we can configure clips in Unity beforehand to call these events ourselves, with the possibility of writing a script that does this automatically.


This guide assumes you have followed the guides on how to set up your initial Outward Mod project https://outward.fandom.com/wiki/Installing_Mods and have also installed Sinai's SideLoader https://sinai-dev.github.io/OSLDocs/#/ - these are both required.

 
 ## Guide 
 
In order to import an AnimationClip correctly into Unity you first need to configure and set up all the animations in a Unity Project, for this example I used the recommended 2018.4.8f1.

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


```C#
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SideLoader;

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

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;

            //we register to the OnSceneLoaded event as we need the character to exist in scene before we can override it's AnimatorController.
            SL.OnSceneLoaded += OnSceneLoaded;

        }

        /// We could technically just assign OverrideClip to the OnSceneLoaded event but I wanted to be explicit in what code does what.
        private void OnSceneLoaded()
        {
            OverrideClip();
        }


        private void OverrideClip()
        {
            //reference animation clip from AssetBundle if you have followed the guide then these names should be familiar to you, the first name is the name of the AssetBundle and the second is the name of the animation asset itself
           
            //reference our SideLoaderPack we created by name (covered in SL docs), I called mine AnimatorOverride
            SLPack sideLoaderPack = SL.GetSLPack("AnimatorOverride");

            //create a new animationclip then set its reference equal to our animation clip in our bundle, make sure we cast this back to an AnimationClip
            AnimationClip newClip = sideLoaderPack.AssetBundles["capoeira"].LoadAsset<AnimationClip>("capoeira");

           //create new animation override controller from base runTimeAnimatorController
            AnimatorOverrideController animationOverrideController = new AnimatorOverrideController(GetPlayerCharacter().Animator.runtimeAnimatorController);


            //find the HumanIdleNeutral_a clip (there is a list on the GitHub of all animations)
            AnimationClip originalClip = GetClipByName(animationOverrideController, "HumanIdleNeutral_a");

            //override the clip in the AnimatorOverrideController
            animationOverrideController.SetClip(originalClip, newClip, true);

            //assign the new AnimatorOverrideController to the runTimeAnimatorController property
            GetPlayerCharacter().Animator.runtimeAnimatorController = animationOverrideController;

        }



        /// <summary>
        /// Finds and returns a AnimationClip by name in the AnimatorOverrideController
        /// </summary>
        /// <param name="overrideController"></param>
        /// <param name="clipName"></param>
        /// <returns></returns>
        private AnimationClip GetClipByName(AnimatorOverrideController overrideController, string clipName)
        {
            Log.LogMessage($"Founding  clip with {clipName} name");

            foreach (var item in overrideController.animationClips)
            {
                if (item.name == clipName)
                {
                    return item;
                }
            }


            Log.LogMessage($"Found no clip with {clipName}");
            return null;
        }


        //this is not a good way to get the character reference!! 
        //I've been away awhile and this was the quickest method I could find at short notice :D
        private Character GetPlayerCharacter()
        {
            return CharacterManager.Instance.Characters.m_values[0];
        }
}
}```

