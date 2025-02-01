using RoR2;
using UnityEngine;
using RoR2.Projectile;
using AdamMod.Modules;

namespace AdamMod.Content
{
    public static class Assets
    {
        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
        }
    }
}
