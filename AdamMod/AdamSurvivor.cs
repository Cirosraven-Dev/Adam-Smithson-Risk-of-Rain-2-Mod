using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using AdamMod.Modules.BaseContent.Characters;
using AdamMod.Modules;
using AdamMod.Components;
using AdamMod.Content;
using EntityStates;
using AdamMod.SkillStates;

namespace AdamMod
{
    public class AdamSurvivor : SurvivorBase<AdamSurvivor>
    {
        public const string MOD_PREFIX = AdamPlugin.DEVELOPER_PREFIX + "_ADAM_";

        public override string assetBundleName => "adamassetbundle";
        public override string bodyName => "AdamBody";
        public override string masterName => "AdamMonsterMaster";
        public override string modelPrefabName => "mdlAdam";
        public override string displayPrefabName => "AdamDisplay";
        public override string survivorTokenPrefix => MOD_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = MOD_PREFIX + "NAME",
            subtitleNameToken = MOD_PREFIX + "SUBTITLE",
            bodyNameToClone = "Commando",

            characterPortrait = assetBundle.LoadAsset<Texture>("AdamPortrait"),
            bodyColor = new Color32(180, 115, 75, 255),
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            //main stats
            maxHealth = 160f,
            healthRegen = 2.5f,
            armor = 20f,
            shield = 0f, // base shield is a thing apparently. neat

            jumpCount = 1,

            //conventional base stats, consistent for all survivors
            damage = 12f,
            attackSpeed = 1f,
            crit = 1f,

            //misc stats
            moveSpeed = 7f,
            acceleration = 80f,
            jumpPower = 15f,

            //stat growth
            /// <summary>
            /// Leave this alone, and you don't need to worry about setting any of the stat growth values. They'll be set at the consistent ratio that all vanilla survivors have.
            /// <para>If You do, healthGrowth should be maxHealth * 0.3f, regenGrowth should be healthRegen * 0.2f, damageGrowth should be damage * 0.2f</para>
            /// </summary>
            autoCalculateLevelStats = true,

            healthGrowth = 100f * 0.3f,
            regenGrowth = 1f * 0.2f,
            armorGrowth = 0f,
            shieldGrowth = 0f,

            damageGrowth = 12f * 0.2f,
            attackSpeedGrowth = 0f,
            critGrowth = 0f,

            moveSpeedGrowth = 0f,
            jumpPowerGrowth = 0f,// jump power per level exists for some reason
        };
        /*
        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = ,
                }
        };*/

        public override UnlockableDef characterUnlockableDef => Unlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new AdamItemDisplays();

        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            Unlockables.Init();

            base.InitializeCharacter();

            Content.Config.Init();
            States.Init();
            Content.Tokens.Init();

            Content.Assets.Init(assetBundle);
            Buffs.Init(assetBundle);
            DamageTypes.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "StabHitboxGroup", "StabHitbox");
        }

        public override void InitializeEntityStateMachines()
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(MainState), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your AdamStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);

            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = MOD_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = MOD_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = MOD_PREFIX + "KEYWORD_IMPALE",
                icon = assetBundle.LoadAsset<Sprite>("Adam_passive"),
            };
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

//            skillLocator.primary = CreateSkill("Bloody Knuckles",
//                "Melee punch dealing 500% damage, heals for 2% missing HP.", "skill_primary.png");

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            var primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "AdamThrustCombo",
                    MOD_PREFIX + "PRIMARY_THRUST_NAME",
                    MOD_PREFIX + "PRIMARY_THRUST_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("Adam_primary"),
                    new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

//            skillLocator.secondary = CreateSkill("Noir Pistol",
//                "Fires 12 shots, 1000% damage each, 10s cooldown, slows enemies by 5%.", "skill_secondary.png");

            //here is a basic skill def with all fields accounted for
            var secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AdamQuickStep",
                skillNameToken = MOD_PREFIX + "SECONDARY_QUICKSTEP_NAME",
                skillDescriptionToken = MOD_PREFIX + "SECONDARY_QUICKSTEP_DESCRIPTION",
                keywordTokens = new string[] { MOD_PREFIX + "KEYWORD_PREPARE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("Adam_secondary_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
            var secondarySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AdamThunderStep",
                skillNameToken = MOD_PREFIX + "SECONDARY_THUNDERSTEP_NAME",
                skillDescriptionToken = MOD_PREFIX + "SECONDARY_THUNDERSTEP_DESCRIPTION",
                keywordTokens = new string[] { MOD_PREFIX + "KEYWORD_PREPARE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("Adam_secondary_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1, secondarySkillDef2);
        }

        private void AddUtilitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

//            skillLocator.utility = CreateSkill("Parasitic Bloom",
//                "Places a healing plant that grants 20% HP per kill. One plant at a time.", "skill_utility.png");

            //here's a skilldef of a typical movement skill.
            var utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AdamSkewer",
                skillNameToken = MOD_PREFIX + "UTILITY_SKEWER_NAME",
                skillDescriptionToken = MOD_PREFIX + "UTILITY_SKEWER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Adam_utility_1"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            var utilitySkillDef2 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AdamReposte",
                skillNameToken = MOD_PREFIX + "UTILITY_RIPOSTE_NAME",
                skillDescriptionToken = MOD_PREFIX + "UTILITY_RIPOSTE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Adam_utility_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1, utilitySkillDef2);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

//            skillLocator.special = CreateSkill("Antique Telephone",
//                "Gain 10x health and damage for 1 minute, then suffer a 25% slow debuff for 10s.",
//                "skill_special.png", 60f);

            //a basic skill. some fields are omitted and will just have default values
            var specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AdamPylon",
                skillNameToken = MOD_PREFIX + "SPECIAL_PYLON_NAME",
                skillDescriptionToken = MOD_PREFIX + "SPECIAL_PYLON_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Adam_special"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.Idle)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            var skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            var childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            var defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            var skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            var defaultSkin = Skins.CreateSkinDef(MOD_PREFIX + "DEFAULT_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("Adam_Base"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            /*defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "meshAdamSword",
                "meshAdamGun",
                "meshAdam");
            */
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Girder"),
                    shouldActivate = true,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MasterySword"),
                    shouldActivate = false,
                }
            };
            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(MOD_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("Adam_Mastery"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);
                //Unlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshAdamSwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshAdamAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.

            var masteryMat = assetBundle.LoadMaterial("matMastery");
            var swordTransform = childLocator.FindChild("MasterySword");
            for (int i = 0; i < masterySkin.rendererInfos.Length; i++)
            {
                if (masterySkin.rendererInfos[i].renderer.transform == swordTransform)
                    masterySkin.rendererInfos[i].defaultMaterial = assetBundle.LoadMaterial("matMasterySword");
                else
                    masterySkin.rendererInfos[i].defaultMaterial = masteryMat;
            }

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Girder"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MasterySword"),
                    shouldActivate = true,
                }
            };
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            AI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
        }
    }
}