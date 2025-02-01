using RoR2;
using UnityEngine;
using AdamMod.Achievements;

namespace AdamMod.Content
{
    public static class Unlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                AdamMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(AdamMasteryAchievement.identifier),
                AdamSurvivor.instance.assetBundle.LoadAsset<Sprite>("Adam_Mastery"));
        }
    }
}
