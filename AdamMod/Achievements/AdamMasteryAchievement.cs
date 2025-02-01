using RoR2;
using AdamMod.Modules.BaseContent.Achievements;

namespace AdamMod.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class AdamMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = AdamSurvivor.MOD_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = AdamSurvivor.MOD_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => AdamSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}