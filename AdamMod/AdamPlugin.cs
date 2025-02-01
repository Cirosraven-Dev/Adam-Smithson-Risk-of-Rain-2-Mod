using BepInEx;
using R2API.Utils;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

//rename this namespace
namespace AdamMod
{
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class AdamPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.josh.adamsmithson";
        public const string MODNAME = "AdamSmithsonMod";
        public const string MODVERSION = "0.0.1";

        public const string DEVELOPER_PREFIX = "JOSH";

        public static AdamPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            Modules.Language.Init();
            new AdamSurvivor().Initialize();
            new Modules.ContentPacks().Initialize();
        }
    }
}
