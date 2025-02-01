using System;
using AdamMod.Achievements;
using AdamMod.Modules;

namespace AdamMod.Content
{
    public static class Tokens
    {
        public static void Init()
        {
            AddAdamTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Adam.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddAdamTokens()
        {
            var desc = "Adam is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            var outro = "..and so he left, with dreams finally fulfilled.";
            var outroFailure = "..and so he vanished, leaving more than just his dreams behind.";

            Add("KEYWORD_PREPARE", Modules.Tokens.KeywordText("Prepare", "Prepare yourself."));
            Add("KEYWORD_IMPALE", Modules.Tokens.KeywordText("Impale", "Impale your enemies."));

            Add("NAME", "Adam");
            Add("DESCRIPTION", desc);
            Add("SUBTITLE", "The Chosen One");
            Add("LORE", "sample lore");
            Add("OUTRO_FLAVOR", outro);
            Add("OUTRO_FAILURE", outroFailure);

            #region Skins
            Add("DEFAULT_SKIN_NAME", "Default");
            Add("MASTERY_SKIN_NAME", "Mastery");
            #endregion

            #region Passive
            Add("PASSIVE_NAME", "Opportunist");
            Add("PASSIVE_DESCRIPTION", Modules.Tokens.impalePrefix + "Gain stacks from Primary and Secondary up to 5 Stacks. Each stack Increases \"Impale\" damage by 25%, armor by 5, and attack speed by 7.5%. Lose stacks while out of combat.(1  Stack every 3?-10? seconds out of combat. will experiment with this one)");
            #endregion

            #region Primary
            Add("PRIMARY_THRUST_NAME", "Thrust");
            Add("PRIMARY_THRUST_DESCRIPTION", Modules.Tokens.agilePrefix + $"Stab forward for 240% damage (1.0 Proc), Cycles between two stab animations. On kill Impale forward for 500% damage (1.0 Proc). Evey 5 Hits Give 1 Passive Stack.");
            #endregion

            #region Secondary
            Add("SECONDARY_QUICKSTEP_NAME", "Quick Step");
            Add("SECONDARY_QUICKSTEP_DESCRIPTION", Modules.Tokens.preparePrefix + $"Sidestep, Gaining invincibility, and adding 1 Passive stack. then Lunge forward towards your crosshair for 390% (1.0 Proc). On kill Impale forward for 500% damage (1.0 Proc).");
            
            Add("SECONDARY_THUNDERSTEP_NAME", "Thunder Step");
            Add("SECONDARY_THUNDERSTEP_DESCRIPTION", Modules.Tokens.preparePrefix + $"Blast upwards, Gaining invincibility, adding 1 Passive stack, and causing a small 300% (1.0 Proc) blast. Then lunge forward towards your crosshair for 390% (1.0 Proc)");
            #endregion

            #region Utility
            Add("UTILITY_SKEWER_NAME", "Skewer");
            Add("UTILITY_SKEWER_DESCRIPTION", "Charge up a Piercing Stab for 600%-2500% (72-288 Damage Base Level. 1.0 Proc). Damage increases by 25% Per enemy hit after the first \r\n288 - 1, 294 - 3, 300 - 5, 306 - 7, 312 - 9, 318 - 11, 324 - 13. It would Take 13 Enemies To match \"Riposte\"'s Damage. Any more than than is a net buff");

            Add("UTILITY_RIPOSTE_NAME", "Skewer");
            Add("UTILITY_RIPOSTE_DESCRIPTION", "Briefly Guard while charging up a Piercing Stab for 700%-2800% (84-324 Damage Base Level. 1.0 Proc). Damage Scales based on how much damage you block, up to 70% of your HP blocked. Attack releases after 2-4? seconds of being held. (Will have to test timing)");
            #endregion

            #region Special
            Add("SPECIAL_PYLON_NAME", "G303 Compaction Pylon");
            Add("SPECIAL_PYLON_DESCRIPTION", $"Toss out a \"Compaction Pylon\" which explodes nearby enemies Dealing 330% (1.0 Proc) and pulling all damaged enemies to the center of the Detonation area");

            Add("SPECIAL_EYES_NAME", "Go For The Eyes");
            Add("SPECIAL_EYES_DESCRIPTION", Modules.Tokens.impalePrefix + $"Ready your weapon, revealing critical spots on nearby enemies, and slowing your fall speed, then make up to 2 targeted strikes for 2x390%(maybe a bit higher). Critical hits reset secondary skill cooldown. additionally Critical Kills Reduce Special Skill cooldown by 2 seconds");
            #endregion

            #region Achievements
            Language.Add(Modules.Tokens.GetAchievementNameToken(AdamMasteryAchievement.identifier), "Adam: Mastery");
            Language.Add(Modules.Tokens.GetAchievementDescriptionToken(AdamMasteryAchievement.identifier), "As Adam, beat the game or obliterate on Monsoon.");
            #endregion
        }

        private static void Add(string token, string text) => Language.Add(AdamSurvivor.MOD_PREFIX + token.Replace(" ", "_").ToUpper(), text);
    }
}
