# OutwardAnimationOverride
A simple example of how to use Animator Override Controllers to change animations in Outward.

This guide assumes you have followed the guides on how to set up your initial Outward Mod project https://outward.fandom.com/wiki/Installing_Mods and have also installed Sinai's SideLoader https://sinai-dev.github.io/OSLDocs/#/ - these are both required.

 
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

Once built navigate to the Output Path folder you set, inside here you will find the bundle you created with the same file name as the AssetBundle you created, this is the only file you need you can ignore the .manifest file. 

![image](https://user-images.githubusercontent.com/3288858/152447079-8dc00268-c0de-4eb6-a3fe-d671905ff24a.png)


In this example I changed the HumanIdle animation to Capoeira dancing, so my AssetBundle was named Capoeira and the Animation **inside** that bundle is also called Capoeria.




Now you need to copy this asset bundle into your Mod folder *inside your r2modman profile folder* 


Here I created a folder called "AnimatorOverride" inside my BepInEx/plugins folder and within that I created a folder called SideLoader as per the SideLoader instructions, then finally inside the SideLoader folder create another Folder called AssetBundles and place your AssetBundle file in here. 

![image](https://user-images.githubusercontent.com/3288858/152447249-b4d3778b-abae-4a50-a30c-dc9759071462.png)


Example : "TestProfile/BepInEx/plugins/YourModFolder/SideLoader/AssetBundles/"
