using System;
using UnityEngine;

namespace AdamMod
{
    internal static class StaticValues
    {
        public static string GetName(ChildNames name) => Enum.GetName(typeof(ChildNames), name);

        public enum ChildNames
        {
            MainHurtbox,
            BodySling,
            Belt,
            Harness,
            ArmWireR,
            ArmWireL,
            ArmLight,
            SuitShoulderR,
            ConnectorR,
            SuitHand,
            WristWire,
            SuitShoulderL,
            Girder,
            Handle,
            Body,
            Scarf,
            Shoulders,
            Head,
            ExoSuit,
            Sling,
            Chestplate,
            Scarf1,
            SuitArmR,
            Kneepads,
            Scarf2,
            HandWire1,
            HandWire2,

        }

        #region Layers
        public const string LAYER_FULLBODY = "FullBody, Override";
        public const string LAYER_GESTURE = "Gesture, Override";
        public const string LAYER_IDLE = "Idle, Additive";

        public const string LAYER_BODY = "Body";
        public const string LAYER_IMPACT = "Impact";
        public const string LAYER_AIM_PITCH = "AimPitch";
        public const string LAYER_AIM_YAW = "AimYaw";
        public const string LAYER_FLINCH = "Flinch";
        #endregion

        #region Parameters
        public const string PARAM_FORWARD_SPEED = "forwardSpeed";
        public const string PARAM_RIGHT_SPEED = "rightSpeed";
        public const string PARAM_UP_SPEED = "upSpeed";
        public const string PARAM_WALK_SPEED = "walkSpeed";
        public const string PARAM_IS_MOVING = "isMoving";
        public const string PARAM_IS_SPRINTING = "isSprinting";
        public const string PARAM_IS_GROUNDED = "isGrounded";
        public const string PARAM_IN_COMBAT = "inCombat";
        public const string PARAM_AIM_YAW = "aimYawCycle";
        public const string PARAM_AIM_PITCH = "aimPitchCycle";
        public const string PARAM_EMOTE_RATE = "Emote.playbackRate";
        public const string PARAM_SLASH_RATE = "Slash.playbackRate";
        public const string PARAM_THROW_RATE = "ThrowBomb.playbackRate";
        public const string PARAM_FLINCH_INDEX = "flinchIndex";
        #endregion

        public const string NAME = "Adam";
        #region Skill Values
        public const float OPPORTUNIST_DMG_MULT = 0.25f;
        #endregion

        #region Anim Control
        public const string EMPTY = "BufferEmpty";
        #endregion

        #region Skill Anims
        //Primary
        public const string PRIMARY_1 = "Primary1";
        public const string PRIMARY_2 = "Primary2";

        // Default Secondary
        public const string DASH = "Dash"; // 1st half of secondary

        // On Kill Impale
        public const string IMPALE = "Impale"; // on kill impale, and for second half of secondary

        // Alt Secondary - Not Made Yet
        //public const string BLAST = "Blast"; // Alt special Jump

        // Default Utility
        public const string STAB_START = "StabStart"; // Start charging
        public const string STAB_HOLD = "StabHold"; // Hold charge
        public const string STAB_END = "StabEnd"; // Release key

        // Alt Utility - Not Made Yet
        //public const string BLOCK_START = "BlockStart"; // Start block
        //public const string BLOCK_HOLD = "BlockHold"; // Hold block
        //public const string BLOCK_END = "BlockEnd"; // On key release, or when held to long

        // Default Special
        public const string THROW = "Throw"; // Default special

        // Alt Special - Not Made Yet
        //public const string AIM = "SpecAim"; // Aim for alt special
        //public const string THROW2 = "SpecStab"; // Attack / primary replacement for Alt special
        #endregion
    }
}