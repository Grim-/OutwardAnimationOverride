using System;

namespace OutwardAOCTest
{
    public static class AnimationsHelper
    {
        public static string Default_Idle = "HumanIdleNeutral_a";
        public static string Default_Walk_Forward = "PaulRun110Sho_a";
        public static string Default_Walk_Right = "HumanRunRight_a";
        public static string Default_Walk_Left = "HumanRunLeft_a";
        public static string Default_Walk_Back = "HumanRunBack_a";
        public static string Default_TurnRight = "PaulTurnRightSho_a";
        public static string Default_TurnLeft = "PaulTurnLeftSho_a";


        public static string Default_Run_Forward = "HumanRun_a";
        public static string Default_Run_Right = "HumanRunRight_a";
        public static string Default_Run_Left = "HumanRunLeft_a";
        public static string Default_Run_Back = "HumanRunBack_a";

        public static string Default_Sprint_Forward = "HumanSprint_a";

        public static string[] DefaultState_Movement =
        {
            Default_Idle,

            Default_Walk_Forward,
            Default_Walk_Right,
            Default_Walk_Left,
            Default_Walk_Back,

            Default_Run_Forward,
            Default_Run_Right,
            Default_Run_Left,
            Default_Run_Back,

            Default_TurnRight,
            Default_TurnLeft,

            Default_Sprint_Forward
        };

        //crouching
        public static string Crouch_Idle = "HumanCrouchIdle_a";
        public static string Crouch_Walk_Forward = "HumanCrouchWalk_a";
        public static string Crouch_Walk_Right = "HumanCrouchWalkRight_a";
        public static string Crouch_Walk_Left = "HumanCrouchWalkLeft_a";
        public static string Crouch_Walk_Back = "HumanCrouchWalkBack_a";
        public static string Crouch_TurnRight = "HumanCrouchTurnRight_a";
        //assuming this is left as there is no crouch left turn 
        public static string Crouch_TurnLeft = "HumanCrouchTurn_a";

        public static string Crouch_Run_Forward = "HumanCrouchSprint_a";
        public static string Crouch_Run_Right = "HumanCrouchRunRight_a";
        public static string Crouch_Run_Left = "HumanCrouchRunLeft_a";
        public static string Crouch_Run_Back = "HumanCrouchRunBack_a";

        public static string Crouch_Sprint_Forward = "HumanCrouchSprint_a";

        private static string[] CrouchState_Movement =
        {
            Crouch_Idle,

            Crouch_Walk_Forward,
            Crouch_Walk_Right,
            Crouch_Walk_Left,
            Crouch_Walk_Back,

            Crouch_Run_Forward,
            Crouch_Run_Right,
            Crouch_Run_Left,
            Crouch_Run_Back,

            Crouch_TurnRight,
            Crouch_TurnLeft,

            Crouch_Sprint_Forward
        };

        //tried
        public static string Tired_Idle = "HumanIdleTired_a";
        public static string Tired_Walk_Forward = "HumanRunTired_a";
        public static string Tired_Walk_Right = "HumanRunRightTired_a";
        public static string Tired_Walk_Left = "HumanRunLeftTired_a";
        public static string Tired_Walk_Back = "HumanRunBack_a";
        public static string Tired_TurnRight = "HumanTurnRightTired_a";
        public static string Tired_TurnLeft = "HumanTurnRightTiredMirror_a";


        public static string Tired_Run_Forward = "HumanSprintTired_a";
        public static string Tired_Run_Right = "HumanRunTiredRight_a";
        public static string Tired_Run_Left = "HumanRunTiredLeft_a";
        public static string Tired_Run_Back = "HumanRunBackTired_a";

        private static string Tired_Sprint_Forward = "HumanSprintTired_a";

        private static string[] TiredState_Movement =
        {
            Tired_Idle,

            Tired_Walk_Forward,
            Tired_Walk_Right,
            Tired_Walk_Left,
            Tired_Walk_Back,

            Tired_Run_Forward,
            Tired_Run_Right,
            Tired_Run_Left,
            Tired_Run_Back,

            Tired_TurnRight,
            Tired_TurnLeft,

            Tired_Sprint_Forward
        };


        public static string Default_Roll = "PaulRollSho_a";
        public static string Attack_Unarmed = "HumanNoWeaponPunch_a";

        private static string[] Actions_Rolling =
        {
            "PaulRollSho_a", "HumanRollBag_a", "HumanRollBuffed_a",
            "HumanRollBack_a", "HumanRollLeft_a", "HumanRollRight_a"
        };

        //Some almost self explanatory actions
        private static string[] Actions =
        {
            "PaulPickUpBag_a", "PaulDropBag_a", "PaulPutInBagLeft_a", "PaulPutInBagRight_a", "PaulSetUpGroundSho_a", "PaulPackUpGroundSho_a", "PaulThrowLanterne_a", "PaulDrinkPotionSho_a", "HumanLightWithFlint_a",
            "HumanBandagin_a", "HumanDrink_a", "HumanGetUpFromBelly_a", "HumanEat_a", "HumanUseUp_a", "PaulGathering_a", "PaulFieldDressing_a", "HumanMining_a", "HumanMiningEnd_a", "HumanSitGroundDown1_a", "HumanSitGroundIdle1_a",
            "HumanSitGroundUp1_a", "HumanIdleArmsCross_a", "HumanPrayingIn_a", "HumanPrayingInn_a", "HumanPrayingInOut_a", "HumanSleepGoTo_a", "HumanSleepIdle_a", "HumanSleepGetUpFrom_a"
        };
    }

}