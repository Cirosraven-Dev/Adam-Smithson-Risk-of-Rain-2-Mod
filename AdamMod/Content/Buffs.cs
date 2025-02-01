using AdamMod.Modules;
using RoR2;
using UnityEngine;

namespace AdamMod.Content
{
    public static class Buffs
    {
        public static BuffDef Darkness;
        public static void Init(AssetBundle assetBundle)
        {
            Darkness = ScriptableObject.CreateInstance<BuffDef>();
            Darkness.isDOT = false;
            Darkness.isCooldown = false;
            Darkness.buffColor = Color.red;
            Darkness.canStack = false;
            Darkness.iconSprite = null;
            Darkness.ignoreGrowthNectar = false;
            Darkness.flags = BuffDef.Flags.ExcludeFromNoxiousThorns;

            Modules.Content.AddBuffDef(Darkness);

            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!sender)
                return;

            if (sender.HasBuff(Darkness))
            {
                args.moveSpeedMultAdd += 0.15f;
                args.critAdd += 20f;
                args.critDamageMultAdd += 0.2f;
            }
        }
    }
}
